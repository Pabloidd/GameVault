using GameVault.Models;
using GameVault.Options;
using Microsoft.Extensions.Options;

namespace GameVault.Repositories
{
    public class PlayerRepository : AbstractRepository, IPlayerRepository
    {
        public PlayerRepository(IOptions<MariaDbOptions> options) : base(options.Value.ConnectionString){}
        
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            string sql = "CALL GetAllPlayers()";
            return await QueryWithConnectionAsync<Player>(sql);
        }
    
        public async Task CreatePlayerAsync(string playerName)
        {
            string sql = $"CALL CreatePlayer({playerName})";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task UpdatePlayerAsync(string oldPlayerName, string newPlayerName)
        {
            string sql = $"CALL UpdatePlayer({oldPlayerName}, {newPlayerName})";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task DeletePlayerAsync(string playerName)
        {
            string sql = $"CALL DeletePlayer({playerName})";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task<List<Player>> GetPlayersSliceAsync(int offset)
        {
            string sql = $"CALL GetPlayersSlice({offset})";
            return await QueryWithConnectionAsync<Player>(sql);
        }
    
        public async Task<List<Player>> GetGamesByPlayerAsync(string playerName)
        {
            string sql = $"CALL GetGamesByPlayer({playerName})";
            return await QueryWithConnectionAsync<Player>(sql);
        }
    
        public async Task<Player?> GetPlayerByNicknameAsync(string nickname)
        {
            string sql = $"CALL GetPlayerByNickname({nickname})";
            return await QuerySingleWithConnectionAsync<Player>(sql);
        }
    }
}