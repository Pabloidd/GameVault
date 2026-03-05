🎮 GameVault - API для работы с БД каталога игр
📋 Описание проекта

GameVault - это REST API для управления каталогом компьютерных игр. Проект реализует полный CRUD функционал для работы с играми, жанрами, издателями и игроками.
🚀 Технологии

    .NET  - платформа разработки

    ASP.NET Core - создание Web API

    MariaDB - база данных

    Dapper - micro ORM для работы с БД

    Хранимые процедуры - вся логика работы с данными

📁 Структура проекта
text
```
GameVault/
├── Controllers/           # Контроллеры API
│   ├── BaseController.cs  # Базовый контроллер с общей логикой
│   ├── CountriesController.cs
│   ├── GamesController.cs
│   ├── GenresController.cs
│   ├── PlayersController.cs
│   ├── PublishersController.cs
│   └── StatisticsController.cs
├── Models/                # Модели данных (соответствуют таблицам БД)
│   ├── Country.cs
│   ├── Game.cs
│   ├── Genre.cs
│   ├── Player.cs
│   ├── Publisher.cs
│   └── DatabaseStatistics.cs
├── Repositories/          # Репозитории для работы с БД
│   ├── AbstractRepository.cs  # Базовый репозиторий
│   ├── ICountryRepository.cs
│   ├── CountryRepository.cs
│   ├── IGameRepository.cs
│   ├── GameRepository.cs
│   ├── IGenreRepository.cs
│   ├── GenreRepository.cs
│   ├── IPlayerRepository.cs
│   ├── PlayerRepository.cs
│   ├── IPublishersRepository.cs
│   ├── PublishersRepository.cs
│   ├── IStatisticsRepository.cs
│   └── StatisticsRepository.cs
├── RequestModels/         # Модели запросов (для POST/PUT)
│   ├── CreateGameRequest.cs
│   ├── UpdateGameRequest.cs
│   ├── GameGenreRequest.cs
│   ├── CreatePlayerRequest.cs
│   ├── UpdatePlayerRequest.cs
│   ├── PlayerGameRequest.cs
│   ├── CreatePublisherRequest.cs
│   ├── UpdatePublisherRequest.cs
│   └── UpdateCountryRequest.cs
├── Extensions/            # Методы расширения
│   └── RegistrationManager.cs  # Регистрация сервисов в DI
├── Options/               # Настройки приложения
│   └── MariaDbOptions.cs
└── Program.cs             # Точка входа
```
🗄️ Структура базы данных

Проект использует базу данных GameCatalog со следующими таблицами:

    Countries - страны

    Genres - жанры игр

    Publishers - издатели

    Games - игры

    Players - игроки

    GamesGenresRelations - связь игр и жанров (многие ко многим)

    PlayersGamesRelations - связь игроков и игр (многие ко многим)

🔧 Установка и запуск
Предварительные требования

    .NET 8 SDK

    MariaDB/MySQL сервер

    База данных GameCatalog с хранимыми процедурами

Настройка

    Клонировать репозиторий

bash

git clone https://github.com/Pabloidd/GameVault.git
cd GameVault

    Настроить строку подключения в appsettings.json

json

{
  "MariaDB": {
    "ConnectionString": "Server=localhost;Database=GameCatalog;User Id=your_user;Password=your_password;"
  }
}

    Запустить проект

bash

dotnet run

Сервер запустится по адресу http://localhost:5120


📡 API Endpoints

```
Countries
Метод	Endpoint	Описание
GET	/api/countries	Получить все страны
POST	/api/countries	Создать новую страну
PUT	/api/countries	Обновить страну
DELETE	/api/countries	Удалить страну
Genres
Метод	Endpoint	Описание
GET	/api/genres	Получить все жанры
POST	/api/genres	Создать новый жанр
PUT	/api/genres	Обновить жанр
DELETE	/api/genres	Удалить жанр
Games
Метод	Endpoint	Описание
GET	/api/games	Получить все игры
GET	/api/games/{title}	Получить игру по названию
POST	/api/games	Создать новую игру
PUT	/api/games	Обновить игру
DELETE	/api/games/{title}	Удалить игру
GET	/api/games/slice/{sliceNumber}	Получить срез игр (пагинация)
GET	/api/games/slice/by-genre?sliceNumber=&genreName=	Срез игр по жанру
GET	/api/games/slice/by-publisher?sliceNumber=&publisherName=	Срез игр по издателю
GET	/api/games/by-genre/{genreName}	Все игры жанра
GET	/api/games/by-publisher/{publisherName}	Все игры издателя
POST	/api/games/add-genre	Добавить жанр игре
DELETE	/api/games/remove-genre	Удалить жанр у игры
GET	/api/games/{gameName}/genres	Жанры игры
GET	/api/games/{gameName}/players	Игроки, у которых есть игра
Players
Метод	Endpoint	Описание
GET	/api/players	Получить всех игроков
GET	/api/players/{nickname}	Получить игрока по никнейму
POST	/api/players	Создать нового игрока
PUT	/api/players	Обновить игрока
DELETE	/api/players/{nickname}	Удалить игрока
GET	/api/players/slice/{sliceNumber}	Срез игроков (пагинация)
GET	/api/players/{nickname}/games	Игры игрока
POST	/api/players/add-game	Добавить игру игроку
DELETE	/api/players/remove-game	Удалить игру у игрока
Publishers
Метод	Endpoint	Описание
GET	/api/publishers	Получить всех издателей
GET	/api/publishers/by-country/{country}	Издатели по стране
POST	/api/publishers	Создать издателя
PUT	/api/publishers	Обновить издателя
DELETE	/api/publishers/{company}	Удалить издателя
GET	/api/publishers/slice/{sliceNumber}	Срез издателей
Statistics
Метод	Endpoint	Описание
GET	/api/statistics	Статистика БД (количество записей)

```
🏗️ Архитектурные решения
Базовые классы

AbstractRepository - предоставляет унифицированные методы для работы с хранимыми процедурами:

    ExecuteProcAsync - для INSERT/UPDATE/DELETE

    QueryProcAsync - для SELECT списков

    QuerySingleProcAsync - для SELECT одной записи

BaseController - содержит общую логику контроллеров:

    Методы валидации (RequireString, RequireMin, RequirePositive)

    Методы выполнения с обработкой ошибок (ExecuteGetAsync, ExecuteListAsync, ExecuteAsync)

Особенности реализации

    Вся логика работы с данными вынесена в хранимые процедуры

    Репозитории только вызывают процедуры и маппят результат

    Контроллеры содержат только валидацию и вызовы репозиториев

    Автоматическое управление транзакциями в репозиториях

⚙️ Требования к базе данных

База данных должна содержать хранимые процедуры:

    CRUD операции для всех сущностей

    Процедуры для связей многие-ко-многим

    Процедуры с пагинацией (срезы по 15 записей)

    Процедура статистики


