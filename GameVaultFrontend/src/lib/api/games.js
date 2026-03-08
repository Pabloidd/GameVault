import api from './client';

/**
 * API клиент для работы с разделом «Игры».
 */
export const gamesApi = {
  /** Получить срез игр для бесконечной прокрутки */
  getSlice: (sliceNumber) => api.get(`games/slice/${sliceNumber}`).then(res => res.data),
  
  /** Получить срез игр по жанру */
  getSliceByGenre: (sliceNumber, genreName) => 
    api.get('games/slice/by-genre', { params: { sliceNumber, genreName } }).then(res => res.data),
    
  /** Получить срез игр по издателю */
  getSliceByPublisher: (sliceNumber, publisherName) => 
    api.get('games/slice/by-publisher', { params: { sliceNumber, publisherName } }).then(res => res.data),
  
  /** Создать новую игру */
  create: (payload) => api.post('games', payload),
  
  /** Обновить данные игры */
  update: (payload) => api.put('games', payload),
  
  /** Удалить игру по названию */
  delete: (title) => api.delete(`games/${encodeURIComponent(title)}`),
  
  /** Получить список жанров игры */
  getGenres: (title) => api.get(`games/${encodeURIComponent(title)}/genres`).then(res => res.data),
  
  /** Получить список игроков игры */
  getPlayers: (title) => api.get(`games/${encodeURIComponent(title)}/players`).then(res => res.data),
  
  /** Добавить жанр игре */
  addGenre: (gameName, genreName) => api.post('games/add-genre', { gameName, genreName }),
  
  /** Удалить жанр у игры */
  removeGenre: (gameName, genreName) => api.delete('games/remove-genre', { data: { gameName, genreName } }),

  /** Получить все игры (для списков выбора) */
  getAll: () => api.get('games').then(res => res.data)
};
