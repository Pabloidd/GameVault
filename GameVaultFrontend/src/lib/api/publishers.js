import api from './client';

/**
 * API клиент для работы с разделом «Издатели».
 */
export const publishersApi = {
  /** Получить всех издателей */
  getAll: () => api.get('publishers').then(res => res.data),
  
  /** Создать нового издателя */
  create: (payload) => api.post('publishers', {
    Company: payload.company,
    Country: payload.country
  }),
  
  /** Обновить данные издателя */
  update: (payload) => api.put('publishers', {
    Company: payload.company,
    NewCountry: payload.country
  }),
  
  /** Удалить издателя по названию компании */
  delete: (company) => api.delete(`publishers/${encodeURIComponent(company)}`)
};
