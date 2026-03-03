using GameVault.Models;
using GameVault.Options;
using Microsoft.Extensions.Options;

namespace GameVault.Repositories
{
    public class GameRepository : AbstractRepository, IGameRepository
    {
        public GameRepository(IOptions<MariaDbOptions> options) 
            : base(options.Value.ConnectionString)
        {
        }
        
        public async Task<List<Game>> GetAllGamesAsync()
        {
            string sql = "CALL GetAllGames()";
            return await QueryWithConnectionAsync<Game>(sql);
        }
    
        public async Task CreateGameAsync(string gameName)
        {
            string sql = "CALL CreateGame(@gameName)";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task UpdateGameAsync(string oldGameName, string newGameName)
        {
            string sql = $"CALL UpdateGame({oldGameName}, {newGameName})";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task DeleteGameAsync(string gameName)
        {
            string sql = $"CALL DeleteGame({gameName})";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task<List<Game>> GetGamesSliceAsync(int offset)
        {
            string sql = $"CALL GetGamesSlice({offset})";
            return await QueryWithConnectionAsync<Game>(sql);
        }
    
        public async Task<List<Game>> GetGamesSliceByGenreAsync(int offset, string genreName)
        {
            string sql = $"CALL GetGamesSliceByGenre({offset}, {genreName})";
            return await QueryWithConnectionAsync<Game>(sql);
        }
    
        public async Task<List<Game>> GetGamesSliceByPublisherAsync(int offset, string publisherName)
        {
            string sql = $"CALL GetGamesSliceByPublisher({offset}, {publisherName})";
            return await QueryWithConnectionAsync<Game>(sql);
        }
    
        public async Task<List<Game>> GetGamesByGenreAsync(string genreName)
        {
            string sql = $"CALL GetGamesByGenre({genreName})";
            return await QueryWithConnectionAsync<Game>(sql);
        }
    
        public async Task<List<Game>> GetGamesByPublisherAsync(string publisherName)
        {
            string sql = $"CALL GetGamesByPublisher({publisherName})";
            return await QueryWithConnectionAsync<Game>(sql);
        }
    
        public async Task RemoveGenreFromGameAsync(string gameName, string genreName)
        {
            string sql = $"CALL RemoveGenreFromGame({gameName}, {genreName})";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task<List<Genre>> GetGameGenresAsync(string gameName)
        {
            string sql = $"CALL GetGameGenres({gameName})";
            return await QueryWithConnectionAsync<Genre>(sql);
        }
    
        public async Task<Game?> GetGameByTitleAsync(string title)
        {
            string sql = $"CALL GetGameByTitle({title})";
            return await QuerySingleWithConnectionAsync<Game>(sql);
        }

        public async Task<List<Player>> GetPlayersByGameAsync(string gameName)
        {
            string sql = $"CALL GetPlayersByGame({gameName})";
            return await QueryWithConnectionAsync<Player>(sql);
        }
        public async Task AddGenreToGameAsync(string gameName, string genreName)
        {
            string sql = $"CALL AddGenreToGame({gameName}, {genreName})";
            await ExecuteWithConnectionAsync(sql);
        }
    }
}