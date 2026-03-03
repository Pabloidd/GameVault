using GameVault.Models;

namespace GameVault.Repositories
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task CreatePlayerAsync(string playerName);
        Task UpdatePlayerAsync(string oldPlayerName, string newPlayerName);
        Task DeletePlayerAsync(string playerName);
        Task<List<Player>> GetPlayersSliceAsync(int offset);
        Task<List<Player>> GetGamesByPlayerAsync(string playerName);
        Task<Player?> GetPlayerByNicknameAsync(string nickname);
    }
}