import api from './client';

export const statisticsApi = {
  getStats: () => api.get('statistics').then(res => res.data)
};
