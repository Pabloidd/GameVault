using GameVault.Models;

namespace GameVault.Repositories
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player?> GetPlayerByNicknameAsync(string nickname);
        Task CreatePlayerAsync(string nickname, string email, DateTime registrationDate, int level);
        Task UpdatePlayerAsync(string nickname, string newEmail, int newLevel);
        Task DeletePlayerAsync(string nickname);
        Task<List<Player>> GetPlayersSliceAsync(int sliceNumber);
        Task<List<Game>> GetPlayerGamesAsync(string nickname);
        Task AddGameToPlayerAsync(string nickname, string gameTitle);
        Task RemoveGameFromPlayerAsync(string nickname, string gameTitle);
    }
}