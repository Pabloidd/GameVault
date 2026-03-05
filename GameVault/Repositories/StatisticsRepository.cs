using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameVault.Models;
using GameVault.Options;
using Microsoft.Extensions.Options;
using System.Data;
using Dapper;

namespace GameVault.Repositories
{
    public class StatisticsRepository : AbstractRepository, IStatisticsRepository
    {
        public StatisticsRepository(IOptions<MariaDbOptions> options)
            : base(options.Value.ConnectionString) { }

        public async Task<DatabaseStatistics?> GetDatabaseStatisticsAsync()
        {
            var result = await QuerySingleProcAsync<DatabaseStatistics>("GetDatabaseStatistics");
            return result;
        }
    }
}