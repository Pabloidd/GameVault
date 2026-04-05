using Dapper;
using MySqlConnector;
using System.Data;
using Microsoft.Extensions.Options;
using GameVault.Options;

namespace GameVault.Repositories
{
    /// <summary>
    /// Абстрактный базовый класс для всех репозиториев.
    /// Предоставляет унифицированные методы для работы с хранимыми процедурами в БД MariaDB/MySQL.
    /// Единая точка управления соединениями и транзакциями.
    /// </summary>
    public abstract class AbstractRepository
    {
        /// <summary>
        /// Строка подключения к базе данных MariaDB/MySQL.
        /// </summary>
        protected readonly string _connectionString;

        /// <summary>
        /// Конструктор базового репозитория.
        /// </summary>
        /// <param name="options">Настройки подключения к MariaDB.</param>
        protected AbstractRepository(IOptions<MariaDbOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        /// <summary>
        /// Метод, который управляет соединением и транзакцией.
        /// Принимает функцию, которая будет выполняться внутри транзакции.
        /// Автоматически выполняет Commit при успехе или Rollback при ошибке.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
        /// <param name="action">Асинхронная функция, получающая соединение и транзакцию.</param>
        /// <returns>Результат выполнения action.</returns>
        /// <exception cref="Exception">Пробрасывает исключение от БД при ошибке выполнения.</exception>
        /// <remarks>
        /// Пример использования:
        /// <code>
        /// var result = await ExecuteInTransactionAsync(async (connection, transaction) =>
        /// {
        ///     return await connection.QueryAsync&lt;Game&gt;("GetAllGames", transaction: transaction);
        /// });
        /// </code>
        /// </remarks>
        protected async Task<T> ExecuteInTransactionAsync<T>(
            Func<MySqlConnection, MySqlTransaction, Task<T>> action)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var result = await action(connection, transaction);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Выполняет хранимую процедуру, которая НЕ ВОЗВРАЩАЕТ данные (INSERT, UPDATE, DELETE).
        /// Автоматически управляет транзакцией.
        /// </summary>
        /// <param name="storedProc">Название хранимой процедуры в БД (например, "CreateCountry").</param>
        /// <param name="parameters">Объект с параметрами для процедуры. Имена свойств должны совпадать с именами параметров процедуры (с префиксом @).</param>
        /// <returns>Task, представляющий асинхронную операцию.</returns>
        /// <exception cref="Exception">Пробрасывает исключение от БД при ошибке выполнения.</exception>
        /// <remarks>
        /// Пример использования:
        /// <code>
        /// var parameters = new { p_country = "USA" };
        /// await ExecuteProcAsync("CreateCountry", parameters);
        /// </code>
        /// </remarks>
        protected async Task ExecuteProcAsync(string storedProc, object parameters)
        {
            await ExecuteInTransactionAsync(async (connection, transaction) =>
            {
                await connection.ExecuteAsync(
                    storedProc,
                    parameters,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );
                return true; // заглушка, т.к. Task<T> требует возврата
            });
        }

        /// <summary>
        /// Выполняет хранимую процедуру, которая ВОЗВРАЩАЕТ СПИСОК ОБЪЕКТОВ (SELECT нескольких записей).
        /// Автоматически маппит результат на указанный тип с помощью Dapper.
        /// </summary>
        /// <typeparam name="T">Тип объектов в возвращаемом списке (должен соответствовать структуре результата процедуры).</typeparam>
        /// <param name="storedProc">Название хранимой процедуры в БД (например, "GetAllCountries").</param>
        /// <param name="parameters">Объект с параметрами для процедуры (может быть null для процедур без параметров).</param>
        /// <returns>Список объектов типа T, полученных из БД.</returns>
        /// <exception cref="Exception">Пробрасывает исключение от БД при ошибке выполнения.</exception>
        /// <remarks>
        /// Пример использования:
        /// <code>
        /// // Без параметров
        /// var countries = await QueryProcAsync&lt;Country&gt;("GetAllCountries");
        /// 
        /// // С параметрами
        /// var parameters = new { p_genre = "RPG" };
        /// var games = await QueryProcAsync&lt;Game&gt;("GetGamesByGenre", parameters);
        /// </code>
        /// </remarks>
        protected async Task<List<T>> QueryProcAsync<T>(string storedProc, object? parameters = null)
            where T : class
        {
            return await ExecuteInTransactionAsync(async (connection, transaction) =>
            {
                var result = await connection.QueryAsync<T>(
                    storedProc,
                    parameters,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );
                return result.AsList();
            });
        }

        /// <summary>
        /// Выполняет хранимую процедуру, которая ВОЗВРАЩАЕТ ОДИН ОБЪЕКТ или NULL (SELECT одной записи).
        /// Автоматически маппит результат на указанный тип с помощью Dapper.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта (должен соответствовать структуре результата процедуры).</typeparam>
        /// <param name="storedProc">Название хранимой процедуры в БД (например, "GetPlayerByNickname").</param>
        /// <param name="parameters">Объект с параметрами для процедуры (может быть null для процедур без параметров).</param>
        /// <returns>Объект типа T или null, если запись не найдена.</returns>
        /// <exception cref="Exception">Пробрасывает исключение от БД при ошибке выполнения.</exception>
        /// <remarks>
        /// Пример использования:
        /// <code>
        /// var parameters = new { p_nickname = "witcher_fan" };
        /// var player = await QuerySingleProcAsync&lt;Player&gt;("GetPlayerByNickname", parameters);
        /// if (player == null) 
        /// {
        ///     // Игрок не найден
        /// }
        /// </code>
        /// </remarks>
        protected async Task<T?> QuerySingleProcAsync<T>(string storedProc, object? parameters = null)
            where T : class
        {
            return await ExecuteInTransactionAsync(async (connection, transaction) =>
            {
                return await connection.QueryFirstOrDefaultAsync<T>(
                    storedProc,
                    parameters,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );
            });
        }
    }
}