using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameVault.Models;
using GameVault.Options;
using Microsoft.Extensions.Options;

namespace GameVault.Repositories
{
    /// <summary>
    /// Репозиторий для получения статистики по базе данных.
    /// </summary>
    public class StatisticsRepository : AbstractRepository, IStatisticsRepository
    {
        public StatisticsRepository(IOptions<MariaDbOptions> options)
            : base(options) { }

        /// <summary>
        /// Получает общую статистику базы данных.
        /// </summary>
        /// <returns>Объект с количеством записей во всех таблицах.</returns>
        public async Task<DatabaseStatistics?> GetDatabaseStatisticsAsync()
        {
            return await QuerySingleProcAsync<DatabaseStatistics>("GetDatabaseStatistics");
        }
    }
}