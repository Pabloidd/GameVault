using GameVault.Models;

namespace GameVault.Repositories
{   
    public interface IGameRepository
    {
        Task<List<Game>> GetAllGamesAsync();
        Task<Game?> GetGameByTitleAsync(string title);
        Task CreateGameAsync(string title, string company, decimal weight, DateTime releaseDate);
        Task UpdateGameAsync(string title, string newCompany, decimal newWeight, DateTime newReleaseDate);
        Task DeleteGameAsync(string title);
        Task<List<Game>> GetGamesSliceAsync(int sliceNumber);
        Task<List<Game>> GetGamesSliceByGenreAsync(int sliceNumber, string genreName);
        Task<List<Game>> GetGamesSliceByPublisherAsync(int sliceNumber, string publisherName);
        Task<List<Game>> GetGamesByGenreAsync(string genreName);
        Task<List<Game>> GetGamesByPublisherAsync(string publisherName);
        Task AddGenreToGameAsync(string gameName, string genreName);
        Task RemoveGenreFromGameAsync(string gameName, string genreName);
        Task<List<Genre>> GetGameGenresAsync(string gameName);
        Task<List<Player>> GetPlayersByGameAsync(string gameName);
    }
}