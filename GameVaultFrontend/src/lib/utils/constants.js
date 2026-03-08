/**
 * Глобальные константы и конфигурация таблиц приложения.
 */
export const TABLES = [
  { 
    id: 'countries',
    name: 'Страны',
    icon: '🌍',
    api: 'countries',
    primaryKey: 'name',
    columns: [
      { key: 'name', label: 'Название страны' }
    ],
    fields: [
      { name: 'name', label: 'Название страны', type: 'text', required: true }
    ],
    endpoints: {
      getAll: () => '/countries',
      create: () => '/countries',
      update: () => '/countries',
      delete: (name) => `/countries`
    }
  },
  { 
    id: 'genres',
    name: 'Жанры',
    icon: '🎮',
    api: 'genres',
    primaryKey: 'name',
    columns: [
      { key: 'name', label: 'Название жанра' }
    ],
    fields: [
      { name: 'name', label: 'Название жанра', type: 'text', required: true }
    ],
    endpoints: {
      getAll: () => '/genres',
      create: () => '/genres',
      update: () => '/genres',
      delete: (name) => `/genres`
    }
  },
  { 
    id: 'games',
    name: 'Игры',
    icon: '🎲',
    api: 'games',
    primaryKey: 'title',
    columns: [
      { key: 'title', label: 'Название' },
      { key: 'company', label: 'Издатель' },
      { key: 'weight', label: 'Вес (ГБ)' },
      { key: 'release_date', label: 'Дата релиза' }
    ],
    fields: [
      { name: 'title', label: 'Название', type: 'text', required: true },
      { name: 'company', label: 'Издатель', type: 'text', required: true },
      { name: 'weight', label: 'Вес (ГБ)', type: 'number', required: true },
      { name: 'release_date', label: 'Дата релиза', type: 'date', required: true }
    ],
    specialEndpoints: [
      { 
        label: '🎯 По жанру', 
        action: 'getByGenre',
        prompt: 'Введите название жанра',
        method: 'GET',
        endpoint: (genre) => `/games/by-genre/${encodeURIComponent(genre)}`
      },
      { 
        label: '🏢 По издателю', 
        action: 'getByPublisher',
        prompt: 'Введите название издателя',
        method: 'GET',
        endpoint: (publisher) => `/games/by-publisher/${encodeURIComponent(publisher)}`
      },
      { 
        label: '📋 Жанры игры', 
        action: 'getGenres',
        prompt: 'Введите название игры',
        method: 'GET',
        endpoint: (game) => `/games/${encodeURIComponent(game)}/genres`
      },
      { 
        label: '👥 Игроки игры', 
        action: 'getPlayers',
        prompt: 'Введите название игры',
        method: 'GET',
        endpoint: (game) => `/games/${encodeURIComponent(game)}/players`
      }
    ],
    endpoints: {
      getAll: () => '/games',
      getSlice: (slice) => `/games/slice/${slice}`,
      getByGenre: (genre) => `/games/by-genre/${encodeURIComponent(genre)}`,
      getByPublisher: (publisher) => `/games/by-publisher/${encodeURIComponent(publisher)}`,
      getGenres: (game) => `/games/${encodeURIComponent(game)}/genres`,
      getPlayers: (game) => `/games/${encodeURIComponent(game)}/players`,
      create: () => '/games',
      update: () => '/games',
      delete: (title) => `/games/${encodeURIComponent(title)}`,
      addGenre: () => '/games/add-genre',
      removeGenre: () => '/games/remove-genre'
    }
  },
  { 
    id: 'players',
    name: 'Игроки',
    icon: '👤',
    api: 'players',
    primaryKey: 'nickname',
    columns: [
      { key: 'nickname', label: 'Никнейм' },
      { key: 'email', label: 'Email' },
      { key: 'level', label: 'Уровень' },
      { key: 'registrationDate', label: 'Дата регистрации' }
    ],
    fields: [
      { name: 'nickname', label: 'Никнейм', type: 'text', required: true },
      { name: 'email', label: 'Email', type: 'email', required: true },
      { name: 'level', label: 'Уровень', type: 'number', required: true },
      { name: 'registrationDate', label: 'Дата регистрации', type: 'datetime-local', required: true }
    ],
    specialEndpoints: [
      { 
        label: '🎮 Игры игрока', 
        action: 'getGames',
        prompt: 'Введите никнейм игрока',
        method: 'GET',
        endpoint: (nickname) => `/players/${encodeURIComponent(nickname)}/games`
      }
    ],
    endpoints: {
      getAll: () => '/players',
      getSlice: (slice) => `/players/slice/${slice}`,
      getGames: (nickname) => `/players/${encodeURIComponent(nickname)}/games`,
      create: () => '/players',
      update: () => '/players',
      delete: (nickname) => `/players/${encodeURIComponent(nickname)}`,
      addGame: () => '/players/add-game',
      removeGame: () => '/players/remove-game'
    }
  },
  { 
    id: 'publishers',
    name: 'Издатели',
    icon: '🏢',
    api: 'publishers',
    primaryKey: 'company',
    columns: [
      { key: 'company', label: 'Название компании' },
      { key: 'country', label: 'Страна' }
    ],
    fields: [
      { name: 'company', label: 'Название компании', type: 'text', required: true },
      { name: 'country', label: 'Страна', type: 'text', required: true }
    ],
    specialEndpoints: [
      { 
        label: '🌍 По стране', 
        action: 'getByCountry',
        prompt: 'Введите название страны',
        method: 'GET',
        endpoint: (country) => `/publishers/by-country/${encodeURIComponent(country)}`
      }
    ],
    endpoints: {
      getAll: () => '/publishers',
      getSlice: (slice) => `/publishers/slice/${slice}`,
      getByCountry: (country) => `/publishers/by-country/${encodeURIComponent(country)}`,
      create: () => '/publishers',
      update: () => '/publishers',
      delete: (company) => `/publishers/${encodeURIComponent(company)}`
    }
  }
];

export const getTableConfig = (tableId) => TABLES.find(t => t.id === tableId);