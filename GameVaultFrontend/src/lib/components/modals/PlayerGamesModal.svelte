<script>
  /**
   * @component PlayerGamesModal
   * Модальное окно для управления списком игр игрока.
   * Позволяет:
   * 1. Просматривать текущие игры игрока.
   * 2. Добавлять новые игры из общего списка.
   * 3. Удалять привязанные игры.
   */
  import BaseModal from './BaseModal.svelte';
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { playersApi } from '$lib/api/players';
  import { gamesApi } from '$lib/api/games';
  import { onMount } from 'svelte';
  import { mapError } from '$lib/utils/errorMapper';
  import { fade, fly } from 'svelte/transition';

  const player = tableStore.currentItem;
  let playerGames = $state([]); // Игры текущего игрока
  let allGames = $state([]);    // Все доступные игры в системе
  let loading = $state(true);
  let actionLoading = $state(false);
  let error = $state(null);
  
  let selectedGameTitle = $state('');

  // Загружаем данные при открытии
  onMount(async () => {
    await loadData();
  });

  async function loadData() {
    loading = true;
    error = null;
    try {
      // Загружаем параллельно игры игрока и все существующие игры
      const [pGames, aGames] = await Promise.all([
        playersApi.getGames(player.nickname),
        gamesApi.getAll()
      ]);
      playerGames = pGames;
      allGames = aGames;
    } catch (err) {
      error = mapError(err);
    } finally {
      loading = false;
    }
  }

  /** Добавление игры игроку */
  async function handleAddGame() {
    if (!selectedGameTitle || actionLoading) return;
    
    // Проверяем, нет ли уже этой игры у игрока
    if (playerGames.some(g => g.title === selectedGameTitle)) {
      error = 'Эта игра уже добавлена игроку';
      return;
    }

    actionLoading = true;
    error = null;
    try {
      await playersApi.addGame(player.nickname, selectedGameTitle);
      selectedGameTitle = '';
      await loadData(); // Перезагружаем список
    } catch (err) {
      error = mapError(err);
    } finally {
      actionLoading = false;
    }
  }

  /** Удаление игры у игрока */
  async function handleRemoveGame(gameTitle) {
    if (actionLoading || !confirm(`Удалить игру "${gameTitle}" у игрока?`)) return;

    actionLoading = true;
    error = null;
    try {
      await playersApi.removeGame(player.nickname, gameTitle);
      await loadData();
    } catch (err) {
      error = mapError(err);
    } finally {
      actionLoading = false;
    }
  }

  function formatDate(date) {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('ru-RU', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  // Список игр, которые еще можно добавить (исключая уже добавленные)
  const availableToSelect = $derived(
    allGames.filter(g => !playerGames.some(pg => pg.title === g.title))
  );
</script>

<BaseModal 
  title={`Игры игрока: ${player?.nickname}`} 
  onclose={() => tableStore.closeModal()}
>
  <div class="games-relationship-manager">
    
    <!-- Секция добавления новой игры -->
    <div class="add-relation-section">
      <div class="select-wrapper">
        <select bind:value={selectedGameTitle} disabled={actionLoading || loading}>
          <option value="">Выберите игру для добавления...</option>
          {#each availableToSelect as game}
            <option value={game.title}>{game.title} ({game.company})</option>
          {/each}
        </select>
      </div>
      <button 
        class="add-relation-btn" 
        onclick={handleAddGame}
        disabled={!selectedGameTitle || actionLoading || loading}
      >
        {actionLoading ? '...' : 'Добавить игру'}
      </button>
    </div>

    {#if error}
      <div class="error-msg" transition:fade>{error}</div>
    {/if}

    <div class="content-scroll">
      {#if loading}
        <div class="loader">
          <div class="spinner small"></div>
          <span>Загрузка данных...</span>
        </div>
      {:else}
        {#if playerGames.length === 0}
          <div class="empty-state" in:fade>
            <div class="empty-icon">🎮</div>
            <p>У этого игрока пока нет игр</p>
          </div>
        {:else}
          <div class="games-grid">
            {#each playerGames as game (game.title)}
              <div class="game-item-card" in:fly={{ y: 20, duration: 300 }}>
                <div class="game-icon">🎲</div>
                <div class="game-info">
                  <span class="game-title">{game.title}</span>
                  <span class="game-meta">{game.company} • {game.weight} ГБ</span>
                  <span class="game-date">Получена: {formatDate(game.registrationDate || new Date())}</span>
                </div>
                <button 
                  class="remove-btn" 
                  onclick={() => handleRemoveGame(game.title)}
                  title="Удалить игру у игрока"
                  disabled={actionLoading}
                >
                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 6h18"></path><path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"></path><path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"></path><line x1="10" y1="11" x2="10" y2="17"></line><line x1="14" y1="11" x2="14" y2="17"></line></svg>
                </button>
              </div>
            {/each}
          </div>
        {/if}
      {/if}
    </div>
  </div>
</BaseModal>

<style>
  .games-relationship-manager {
    width: 100%;
    max-width: 500px;
    min-width: 320px;
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .add-relation-section {
    display: flex;
    gap: 0.75rem;
    padding-bottom: 1rem;
    border-bottom: 1px dashed #e2e8f0;
  }

  .select-wrapper {
    flex: 1;
  }

  select {
    width: 100%;
    padding: 0.75rem 1rem;
    border-radius: 12px;
    border: 2px solid #e2e8f0;
    background: #f8fafc;
    color: #1e293b;
    font-size: 0.95rem;
    outline: none;
    cursor: pointer;
    transition: all 0.2s;
  }

  select:focus {
    border-color: #6366f1;
    background: white;
    box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.1);
  }

  .add-relation-btn {
    padding: 0 1.5rem;
    background: #6366f1;
    color: white;
    border: none;
    border-radius: 12px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s;
    white-space: nowrap;
  }

  .add-relation-btn:hover:not(:disabled) {
    background: #4f46e5;
    transform: translateY(-1px);
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.2);
  }

  .add-relation-btn:disabled {
    background: #cbd5e1;
    cursor: not-allowed;
  }

  .content-scroll {
    max-height: 40vh;
    overflow-y: auto;
    padding-right: 0.5rem;
  }

  /* Тонкий скроллбар */
  .content-scroll::-webkit-scrollbar {
    width: 6px;
  }
  .content-scroll::-webkit-scrollbar-thumb {
    background: #e2e8f0;
    border-radius: 10px;
  }

  .games-grid {
    display: flex;
    flex-direction: column;
    gap: 0.75rem;
  }

  .game-item-card {
    display: flex;
    align-items: center;
    gap: 1rem;
    padding: 0.875rem;
    background: white;
    border: 1px solid #e2e8f0;
    border-radius: 14px;
    transition: all 0.2s;
  }

  .game-item-card:hover {
    border-color: #6366f1;
    background: #fdfdff;
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.05);
  }

  .game-icon {
    width: 42px;
    height: 42px;
    background: linear-gradient(135deg, #f0f9ff, #e0f2fe);
    border: 1px solid #bae6fd;
    color: #0369a1;
    border-radius: 10px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.25rem;
    flex-shrink: 0;
  }

  .game-info {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 0.125rem;
  }

  .game-title {
    font-weight: 700;
    color: #1e293b;
    font-size: 0.95rem;
  }

  .game-meta {
    font-size: 0.8rem;
    color: #64748b;
  }

  .game-date {
    font-size: 0.7rem;
    color: #94a3b8;
    margin-top: 0.125rem;
  }

  .remove-btn {
    width: 36px;
    height: 36px;
    display: flex;
    align-items: center;
    justify-content: center;
    background: #fff1f2;
    color: #f43f5e;
    border: 1px solid #fecdd3;
    border-radius: 10px;
    cursor: pointer;
    transition: all 0.2s;
    flex-shrink: 0;
  }

  .remove-btn:hover:not(:disabled) {
    background: #f43f5e;
    color: white;
    border-color: #f43f5e;
  }

  .error-msg {
    background: #fef2f2;
    color: #ef4444;
    padding: 0.75rem 1rem;
    border-radius: 12px;
    font-size: 0.9rem;
    border: 1px solid #fee2e2;
  }

  .loader {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    gap: 1rem;
    padding: 3rem;
    color: #64748b;
  }

  .empty-state {
    text-align: center;
    padding: 3rem 1rem;
    color: #94a3b8;
  }

  .empty-icon {
    font-size: 2.5rem;
    margin-bottom: 0.75rem;
    opacity: 0.5;
  }

  .spinner.small {
    width: 24px;
    height: 24px;
    border: 3px solid #f3f3f3;
    border-top: 3px solid #6366f1;
    border-radius: 50%;
    animation: spin 1s linear infinite;
  }

  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }
</style>
