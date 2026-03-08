import api from './client';

/**
 * API клиент для работы с разделом «Страны».
 */
export const countriesApi = {
  /** Получить все страны */
  getAll: () => api.get('countries').then(res => res.data),
  
  /** Создать новую страну */
  create: (name) => api.post('countries', JSON.stringify(name), { headers: { 'Content-Type': 'application/json' } }),
  
  /** Обновить название страны */
  update: (oldName, newName) => api.put('countries', { OldCountryName: oldName, NewCountryName: newName }),
  
  /** Удалить страну */
  delete: (name) => api.delete('countries', { data: JSON.stringify(name), headers: { 'Content-Type': 'application/json' } })
};
