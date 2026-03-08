import axios from 'axios';

/**
 * Базовая конфигурация Axios для работы с API Backend.
 * @constant {AxiosInstance}
 */
const api = axios.create({
  baseURL: '/api', // Все запросы будут начинаться с /api
  headers: {
    'Content-Type': 'application/json'
  }
});

export default api;
