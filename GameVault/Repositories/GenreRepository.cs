using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameVault.Models;
using GameVault.Options;
using Microsoft.Extensions.Options;

namespace GameVault.Repositories
{
    /// <summary>
    /// Репозиторий для работы с жанрами.
    /// </summary>
    public class GenreRepository : AbstractRepository, IGenreRepository
    {
        public GenreRepository(IOptions<MariaDbOptions> options)
            : base(options) { }

        /// <summary>
        /// Получает список всех жанров.
        /// </summary>
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await QueryProcAsync<Genre>("GetAllGenres");
        }

        /// <summary>
        /// Создаёт новый жанр.
        /// </summary>
        /// <param name="genreName">Название жанра.</param>
        public async Task CreateGenreAsync(string genreName)
        {
            var parameters = new { p_genre = genreName };
            await ExecuteProcAsync("CreateGenre", parameters);
        }

        /// <summary>
        /// Обновляет название жанра.
        /// </summary>
        /// <param name="oldGenreName">Старое название жанра.</param>
        /// <param name="newGenreName">Новое название жанра.</param>
        public async Task UpdateGenreAsync(string oldGenreName, string newGenreName)
        {
            var parameters = new
            {
                p_old_genre = oldGenreName,
                p_new_genre = newGenreName
            };
            await ExecuteProcAsync("UpdateGenre", parameters);
        }

        /// <summary>
        /// Удаляет жанр.
        /// </summary>
        /// <param name="genreName">Название жанра.</param>
        public async Task DeleteGenreAsync(string genreName)
        {
            var parameters = new { p_genre = genreName };
            await ExecuteProcAsync("DeleteGenre", parameters);
        }
    }
}