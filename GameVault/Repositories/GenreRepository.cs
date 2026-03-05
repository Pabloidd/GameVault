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
    public class GenreRepository : AbstractRepository, IGenreRepository
    {
        public GenreRepository(IOptions<MariaDbOptions> options) 
            : base(options.Value.ConnectionString) { }
        
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await QueryProcAsync<Genre>("GetAllGenres");
        }
    
        public async Task CreateGenreAsync(string genreName)
        {
            var parameters = new { p_genre = genreName };
            await ExecuteProcAsync("CreateGenre", parameters);
        }

        public async Task UpdateGenreAsync(string oldGenreName, string newGenreName)
        {
            var parameters = new 
            { 
                p_old_genre = oldGenreName,
                p_new_genre = newGenreName 
            };
            await ExecuteProcAsync("UpdateGenre", parameters);
        }

        public async Task DeleteGenreAsync(string genreName)
        {
            var parameters = new { p_genre = genreName };
            await ExecuteProcAsync("DeleteGenre", parameters);
        }
    }
}