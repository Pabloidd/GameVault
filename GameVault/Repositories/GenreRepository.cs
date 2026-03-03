using GameVault.Models;
using GameVault.Options;
using Microsoft.Extensions.Options;

namespace GameVault.Repositories
{
    public class GenreRepository : AbstractRepository, IGenreRepository
    {
        public GenreRepository(IOptions<MariaDbOptions> options) : base(options.Value.ConnectionString){}
        
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            string sql = "CALL GetAllGenres()";
            return await QueryWithConnectionAsync<Genre>(sql);
        }
    
        public async Task CreateGenreAsync(string genreName)
        {
            string sql = $"CALL CreateGenre({genreName})";
            await ExecuteWithConnectionAsync(sql);
        }

        public async Task UpdateGenreAsync(string oldGenreName, string newGenreName)
        {
            string sql = $"CALL UpdateGenre({oldGenreName}, {newGenreName})";
            await ExecuteWithConnectionAsync(sql);
        }

        public async Task DeleteGenreAsync(string genreName)
        {
            string sql = $"CALL DeleteGenre({genreName})";
            await ExecuteWithConnectionAsync(sql);
        }
    }
}