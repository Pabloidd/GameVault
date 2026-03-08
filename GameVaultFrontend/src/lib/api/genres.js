import api from './client';

/**
 * API клиент для работы с разделом «Жанры».
 */
export const genresApi = {
  /** Получить все жанры */
  getAll: () => api.get('genres').then(res => res.data),
  
  /** Создать новый жанр */
  create: (name) => api.post('genres', JSON.stringify(name), { headers: { 'Content-Type': 'application/json' } }),
  
  /** Обновить название жанра */
  update: (oldName, newName) => api.put('genres', { OldGenreName: oldName, NewGenreName: newName }),
  
  /** Удалить жанр */
  delete: (name) => api.delete('genres', { data: JSON.stringify(name), headers: { 'Content-Type': 'application/json' } })
};
