using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;
using Dapper;
using System.Transactions;

namespace GameVault.Repositories
{
    /// <summary>
    /// Абстрактный базовый класс для всех репозиториев.
    /// Предоставляет унифицированные методы для работы с хранимыми процедурами в БД MariaDB/MySQL.
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
        /// <param name="connectionString">Строка подключения к БД, получаемая из конфигурации.</param>
        protected AbstractRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Выполняет хранимую процедуру, которая НЕ ВОЗВРАЩАЕТ данные (INSERT, UPDATE, DELETE).
        /// Автоматически управляет транзакцией: commit при успехе, rollback при ошибке.
        /// </summary>
        /// <typeparam name="T">Тип параметров (обычно анонимный объект).</typeparam>
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
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            
            try
            {
                await connection.ExecuteAsync(
                    storedProc,
                    parameters,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
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
        protected async Task<List<T>> QueryProcAsync<T>(string storedProc, object? parameters = null) where T : class
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            
            try
            {
                var result = await connection.QueryAsync<T>(
                    storedProc,
                    parameters,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );
                transaction.Commit();
                return result.ToList();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
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
        protected async Task<T?> QuerySingleProcAsync<T>(string storedProc, object? parameters = null) where T : class
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<T>(
                    storedProc,
                    parameters,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}