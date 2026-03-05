using GameVault.Models;

namespace GameVault.Repositories
{
    public interface IStatisticsRepository
    {
        Task<DatabaseStatistics> GetDatabaseStatisticsAsync();
    }
}