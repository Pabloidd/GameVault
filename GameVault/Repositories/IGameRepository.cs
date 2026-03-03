using GameVault.Models;

namespace GameVault.Repositories
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllGamesAsync();
        Task CreateGameAsync(string gameName);
        Task UpdateGameAsync(string oldGameName, string newGameName);
        Task DeleteGameAsync(string gameName);
        Task<List<Player>> GetPlayersByGameAsync(string gameName);
        Task<List<Game>> GetGamesByGenreAsync(string genreName);
        Task<List<Game>> GetGamesByPublisherAsync(string publisherName);
        Task AddGenreToGameAsync(string gameName, string genreName);
        Task RemoveGenreFromGameAsync(string gameName, string genreName);
        Task<List<Genre>> GetGameGenresAsync(string gameName);
        Task<List<Game>> GetGamesSliceAsync(int offset);
        Task<List<Game>> GetGamesSliceByGenreAsync(int offset, string genreName);
        Task<List<Game>> GetGamesSliceByPublisherAsync(int offset, string publisherName);
        Task<Game?> GetGameByTitleAsync(string title);
        

    
    }
}