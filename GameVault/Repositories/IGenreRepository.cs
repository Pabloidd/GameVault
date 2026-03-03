using GameVault.Models;

namespace GameVault.Repositories
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllGenresAsync();
        Task CreateGenreAsync(string genreName);
        Task UpdateGenreAsync(string oldGenreName, string newGenreName);
        Task DeleteGenreAsync(string genreName);
    }
}