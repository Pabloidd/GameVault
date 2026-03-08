import { countriesApi } from '$lib/api/countries';
import { genresApi } from '$lib/api/genres';
import { publishersApi } from '$lib/api/publishers';
import { gamesApi } from '$lib/api/games';
import { playersApi } from '$lib/api/players';
import { TABLES } from '$lib/utils/constants';
import { mapError } from '$lib/utils/errorMapper';

/**
 * Единое хранилище (Store) для управления данными таблиц.
 * Использует Svelte 5 Runes ($state, $derived) для реактивности.
 */
class TableStore {
  /** Идентификатор инстанса для отладки */
  instanceId = Math.random().toString(36).substr(2, 9);
  
  /** Основной массив данных текущей таблицы */
  data = $state([]);
  
  /** Флаг первичной загрузки */
  loading = $state(false);
  
  /** Сообщение об ошибке (если есть) */
  error = $state(null);
  
  /** ID текущей открытой таблицы (напр. 'games', 'players') */
  currentTableId = $state(null);
  
  // Состояние бесконечной прокрутки
  /** Индекс текущего загруженного среза (slice) */
  sliceNumber = $state(0);
  /** Есть ли еще данные для загрузки? */
  hasMore = $state(true);
  /** Флаг подгрузки следующей порции данных */
  loadingNext = $state(false);
  
  constructor() {
    console.log(`TableStore instance created: ${this.instanceId}`);
  }
  
  // Управление модальными окнами
  /** Текущее активное модальное окно: 'create' | 'update' | 'delete' | 'gameGenres' | 'gamePlayers' | 'playerGames' | null */
  activeModal = $state(null); // 'create' | 'update' | 'delete' | null
  /** Объект данных, с которым взаимодействует модал (например, редактируемая игра) */
  currentItem = $state(null);

  /** Возвращает конфигурацию текущей таблицы из констант */
  get currentTable() {
    return TABLES.find(t => t.id === this.currentTableId);
  }

  /** Открыть модальное окно нужного типа */
  openModal(type, item = null) {
    this.activeModal = type;
    this.currentItem = item;
  }

  /** Закрыть активное модальное окно */
  closeModal() {
    this.activeModal = null;
    this.currentItem = null;
  }

  /** 
   * Инициализация хранилища при переходе на новую таблицу.
   * Сбрасывает данные и пагинацию.
   */
  async init(tableId) {
    console.log(`[Store ${this.instanceId}] Initializing with table: ${tableId}`);
    this.currentTableId = tableId;
    this.data = [];
    this.sliceNumber = 0;
    this.hasMore = true;
    this.closeModal();
    await this.fetchData();
  }

  /**
   * Загрузка первичных данных для текущей таблицы.
   * Поддерживает как обычную загрузку (все сразу), так и срезы (slice).
   */
  async fetchData() {
    if (!this.currentTableId) return;

    this.loading = true;
    this.error = null;
    try {
      if (this.currentTableId === 'countries') {
        const result = await countriesApi.getAll();
        this.data = result;
      } else if (this.currentTableId === 'genres') {
        const result = await genresApi.getAll();
        this.data = result;
      } else if (this.currentTableId === 'publishers') {
        const result = await publishersApi.getAll();
        this.data = result;
      } else if (this.currentTableId === 'games') {
        this.sliceNumber = 0;
        const result = await gamesApi.getSlice(0);
        this.data = result;
        this.hasMore = result.length >= 10;
      } else if (this.currentTableId === 'players') {
        this.sliceNumber = 0;
        const result = await playersApi.getSlice(0);
        this.data = result;
        this.hasMore = result.length >= 10;
      }
    } catch (err) {
      this.error = mapError(err, this.currentTableId);
    } finally {
      this.loading = false;
    }
  }

  /** Создание новой записи (универсальный метод) */
  async create(payload) {
    try {
      if (this.currentTableId === 'countries') {
        await countriesApi.create(payload.name);
      } else if (this.currentTableId === 'genres') {
        await genresApi.create(payload.name);
      } else if (this.currentTableId === 'publishers') {
        await publishersApi.create(payload);
      } else if (this.currentTableId === 'players') {
        await playersApi.create(payload);
      }
      this.closeModal();
      await this.fetchData();
    } catch (err) {
      throw err;
    }
  }

  /** Обновление существующей записи (универсальный метод) */
  async update(payload) {
    try {
      if (this.currentTableId === 'countries') {
        await countriesApi.update(this.currentItem.name, payload.name);
      } else if (this.currentTableId === 'genres') {
        await genresApi.update(this.currentItem.name, payload.name);
      } else if (this.currentTableId === 'publishers') {
        await publishersApi.update(payload);
      } else if (this.currentTableId === 'players') {
        await playersApi.update(payload);
      }
      this.closeModal();
      await this.fetchData();
    } catch (err) {
      throw err;
    }
  }

  /** Удаление записи */
  async delete(item) {
    try {
      if (this.currentTableId === 'countries') {
        await countriesApi.delete(item.name);
      } else if (this.currentTableId === 'genres') {
        await genresApi.delete(item.name);
      } else if (this.currentTableId === 'publishers') {
        await publishersApi.delete(item.company);
      } else if (this.currentTableId === 'games') {
        await gamesApi.delete(item.title);
      } else if (this.currentTableId === 'players') {
        await playersApi.delete(item.nickname);
      }
      this.closeModal();
      await this.fetchData();
    } catch (err) {
      throw err;
    }
  }

  /** 
   * Подгрузка следующей страницы (среза) данных.
   * Вызывается при срабатывании Intersection Observer в DataTable.
   */
  async fetchNextSlice() {
    const isSlicable = ['games', 'players'].includes(this.currentTableId);
    if (this.loading || this.loadingNext || !this.hasMore || !isSlicable) return;

    this.loadingNext = true;
    try {
      const nextSlice = this.sliceNumber + 1;
      let result = [];
      
      if (this.currentTableId === 'games') {
        result = await gamesApi.getSlice(nextSlice);
      } else if (this.currentTableId === 'players') {
        result = await playersApi.getSlice(nextSlice);
      }
      
      if (result && result.length > 0) {
        this.data = [...this.data, ...result];
        this.sliceNumber = nextSlice;
        this.hasMore = result.length >= 10;
      } else {
        this.hasMore = false;
      }
    } catch (err) {
      console.error('Failed to fetch next slice:', err);
      this.hasMore = false;
    } finally {
      this.loadingNext = false;
    }
  }

  // Games specific methods
  async createGame(payload) {
    await gamesApi.create(payload);
    this.closeModal();
    await this.fetchData();
  }

  async updateGame(payload) {
    await gamesApi.update(payload);
    this.closeModal();
    await this.fetchData();
  }

  // Players specific methods
  async createPlayer(payload) {
    await playersApi.create(payload);
    this.closeModal();
    await this.fetchData();
  }

  async updatePlayer(payload) {
    await playersApi.update(payload);
    this.closeModal();
    await this.fetchData();
  }
}

// Global singleton to prevent multi-instance issues in Svelte 5/Vite
const STORE_KEY = '__GAMEVAULT_TABLE_STORE__';
if (typeof window !== 'undefined' && !window[STORE_KEY]) {
  window[STORE_KEY] = new TableStore();
}

export const tableStore = typeof window !== 'undefined' ? window[STORE_KEY] : new TableStore();
