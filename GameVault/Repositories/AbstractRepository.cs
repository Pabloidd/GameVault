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
    public abstract class AbstractRepository
    {
        protected readonly string _connectionString;

        protected AbstractRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Для хранимок, которые ничего не возвращают (INSERT, UPDATE, DELETE)
        /// </summary>
        protected async Task ExecuteWithConnectionAsync(string action)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            
            try
            {
                await connection.ExecuteAsync(action);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        
        /// <summary>
        /// Для хранимых процедур, которые возвращают СПИСОК объектов (SELECT нескольких записей)
        /// </summary>
        protected async Task<List<T>> QueryWithConnectionAsync<T>(string sql) where T : class
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            
            try
            {
                var result = await connection.QueryAsync<T>(sql);
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
        /// Для хранимых процедур, которые возвращают ОДИН объект (SELECT одной записи)
        /// </summary>
        protected async Task<T?> QuerySingleWithConnectionAsync<T>(string sql) where T : class
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<T>(sql);
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