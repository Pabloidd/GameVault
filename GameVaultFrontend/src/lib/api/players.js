import api from './client';

/**
 * API клиент для работы с разделом «Игроки».
 */
export const playersApi = {
  /** Получить всех игроков */
  getAll: async () => {
    const response = await api.get('/players');
    return response.data;
  },

  /** 
   * Получить срез игроков для бесконечной прокрутки.
   * @param {number} sliceNumber - Индекс страницы
   */
  getSlice: async (sliceNumber) => {
    const response = await api.get(`/players/slice/${sliceNumber}`);
    return response.data;
  },

  /** Получить список игр, которые есть у игрока */
  getGames: async (nickname) => {
    const response = await api.get(`/players/${encodeURIComponent(nickname)}/games`);
    return response.data;
  },

  /** Создать нового игрока */
  create: async (player) => {
    await api.post('/players', player);
  },

  /** Обновить данные игрока */
  update: async (player) => {
    await api.put('/players', player);
  },

  /** Удалить игрока по никнейму */
  delete: async (nickname) => {
    await api.delete(`/players/${encodeURIComponent(nickname)}`);
  },

  /** Добавить игру игроку (покупка/владение) */
  addGame: async (nickname, gameTitle) => {
    await api.post('/players/add-game', { nickname, gameTitle });
  },

  /** Удалить игру у игрока */
  removeGame: async (nickname, gameTitle) => {
    await api.delete('/players/remove-game', { data: { nickname, gameTitle } });
  }
};
