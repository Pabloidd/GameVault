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
    /// Репозиторий для работы с играми.
    /// </summary>
    public class GameRepository : AbstractRepository, IGameRepository
    {
        public GameRepository(IOptions<MariaDbOptions> options)
            : base(options) { }

        /// <summary>
        /// Получает список всех игр.
        /// </summary>
        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await QueryProcAsync<Game>("GetAllGames");
        }

        /// <summary>
        /// Получает игру по названию.
        /// </summary>
        /// <param name="title">Название игры.</param>
        public async Task<Game?> GetGameByTitleAsync(string title)
        {
            var parameters = new { p_title = title };
            return await QuerySingleProcAsync<Game>("GetGameByTitle", parameters);
        }

        /// <summary>
        /// Создаёт новую игру.
        /// </summary>
        /// <param name="title">Название игры.</param>
        /// <param name="company">Издатель.</param>
        /// <param name="weight">Вес игры в МБ.</param>
        /// <param name="releaseDate">Дата выпуска.</param>
        public async Task CreateGameAsync(string title, string company, decimal weight, DateTime releaseDate)
        {
            var parameters = new
            {
                p_title = title,
                p_company = company,
                p_weight = weight,
                p_release_date = releaseDate
            };
            await ExecuteProcAsync("CreateGame", parameters);
        }

        /// <summary>
        /// Обновляет информацию об игре.
        /// </summary>
        /// <param name="title">Название игры (идентификатор).</param>
        /// <param name="newCompany">Новый издатель.</param>
        /// <param name="newWeight">Новый вес.</param>
        /// <param name="newReleaseDate">Новая дата выпуска.</param>
        public async Task UpdateGameAsync(string title, string newCompany, decimal newWeight, DateTime newReleaseDate)
        {
            var parameters = new
            {
                p_title = title,
                p_new_company = newCompany,
                p_new_weight = newWeight,
                p_new_release_date = newReleaseDate
            };
            await ExecuteProcAsync("UpdateGame", parameters);
        }

        /// <summary>
        /// Удаляет игру.
        /// </summary>
        /// <param name="title">Название игры.</param>
        public async Task DeleteGameAsync(string title)
        {
            var parameters = new { p_title = title };
            await ExecuteProcAsync("DeleteGame", parameters);
        }

        /// <summary>
        /// Получает срез игр (порцию) для бесконечной прокрутки.
        /// </summary>
        /// <param name="sliceNumber">Номер среза (0 = первые 15).</param>
        public async Task<List<Game>> GetGamesSliceAsync(int sliceNumber)
        {
            var parameters = new { p_slice_number = sliceNumber };
            return await QueryProcAsync<Game>("GetGamesSlice", parameters);
        }

        /// <summary>
        /// Получает срез игр по жанру.
        /// </summary>
        /// <param name="sliceNumber">Номер среза.</param>
        /// <param name="genreName">Название жанра.</param>
        public async Task<List<Game>> GetGamesSliceByGenreAsync(int sliceNumber, string genreName)
        {
            var parameters = new
            {
                p_genre = genreName,
                p_slice_number = sliceNumber
            };
            return await QueryProcAsync<Game>("GetGamesSliceByGenre", parameters);
        }

        /// <summary>
        /// Получает срез игр по издателю.
        /// </summary>
        /// <param name="sliceNumber">Номер среза.</param>
        /// <param name="publisherName">Название издателя.</param>
        public async Task<List<Game>> GetGamesSliceByPublisherAsync(int sliceNumber, string publisherName)
        {
            var parameters = new
            {
                p_company = publisherName,
                p_slice_number = sliceNumber
            };
            return await QueryProcAsync<Game>("GetGamesSliceByPublisher", parameters);
        }

        /// <summary>
        /// Получает список игр по жанру (без пагинации).
        /// </summary>
        /// <param name="genreName">Название жанра.</param>
        public async Task<List<Game>> GetGamesByGenreAsync(string genreName)
        {
            var parameters = new { p_genre = genreName };
            return await QueryProcAsync<Game>("GetGamesByGenre", parameters);
        }

        /// <summary>
        /// Получает список игр по издателю (без пагинации).
        /// </summary>
        /// <param name="publisherName">Название издателя.</param>
        public async Task<List<Game>> GetGamesByPublisherAsync(string publisherName)
        {
            var parameters = new { p_company = publisherName };
            return await QueryProcAsync<Game>("GetGamesByPublisher", parameters);
        }

        /// <summary>
        /// Добавляет жанр к игре.
        /// </summary>
        /// <param name="gameName">Название игры.</param>
        /// <param name="genreName">Название жанра.</param>
        public async Task AddGenreToGameAsync(string gameName, string genreName)
        {
            var parameters = new
            {
                p_title = gameName,
                p_genre = genreName
            };
            await ExecuteProcAsync("AddGenreToGame", parameters);
        }

        /// <summary>
        /// Удаляет жанр у игры.
        /// </summary>
        /// <param name="gameName">Название игры.</param>
        /// <param name="genreName">Название жанра.</param>
        public async Task RemoveGenreFromGameAsync(string gameName, string genreName)
        {
            var parameters = new
            {
                p_title = gameName,
                p_genre = genreName
            };
            await ExecuteProcAsync("RemoveGenreFromGame", parameters);
        }

        /// <summary>
        /// Получает список жанров игры.
        /// </summary>
        /// <param name="gameName">Название игры.</param>
        public async Task<List<Genre>> GetGameGenresAsync(string gameName)
        {
            var parameters = new { p_title = gameName };
            return await QueryProcAsync<Genre>("GetGameGenres", parameters);
        }

        /// <summary>
        /// Получает список игроков, у которых есть данная игра.
        /// </summary>
        /// <param name="gameName">Название игры.</param>
        public async Task<List<Player>> GetPlayersByGameAsync(string gameName)
        {
            var parameters = new { p_title = gameName };
            return await QueryProcAsync<Player>("GetPlayersByGame", parameters);
        }
    }
}