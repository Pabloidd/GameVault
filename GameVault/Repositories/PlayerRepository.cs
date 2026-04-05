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
    /// Репозиторий для работы с игроками.
    /// </summary>
    public class PlayerRepository : AbstractRepository, IPlayerRepository
    {
        public PlayerRepository(IOptions<MariaDbOptions> options)
            : base(options) { }

        /// <summary>
        /// Получает список всех игроков.
        /// </summary>
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await QueryProcAsync<Player>("GetAllPlayers");
        }

        /// <summary>
        /// Получает игрока по никнейму.
        /// </summary>
        /// <param name="nickname">Никнейм игрока.</param>
        public async Task<Player?> GetPlayerByNicknameAsync(string nickname)
        {
            var parameters = new { p_nickname = nickname };
            return await QuerySingleProcAsync<Player>("GetPlayerByNickname", parameters);
        }

        /// <summary>
        /// Создаёт нового игрока.
        /// </summary>
        /// <param name="nickname">Никнейм.</param>
        /// <param name="email">Email.</param>
        /// <param name="registrationDate">Дата регистрации.</param>
        /// <param name="level">Уровень игрока.</param>
        public async Task CreatePlayerAsync(string nickname, string email, DateTime registrationDate, int level)
        {
            var parameters = new
            {
                p_nickname = nickname,
                p_email = email,
                p_registration_date = registrationDate,
                p_level = level
            };
            await ExecuteProcAsync("CreatePlayer", parameters);
        }

        /// <summary>
        /// Обновляет данные игрока.
        /// </summary>
        /// <param name="nickname">Никнейм (идентификатор).</param>
        /// <param name="newEmail">Новый email.</param>
        /// <param name="newLevel">Новый уровень.</param>
        public async Task UpdatePlayerAsync(string nickname, string newEmail, int newLevel)
        {
            var parameters = new
            {
                p_nickname = nickname,
                p_new_email = newEmail,
                p_new_level = newLevel
            };
            await ExecuteProcAsync("UpdatePlayer", parameters);
        }

        /// <summary>
        /// Удаляет игрока.
        /// </summary>
        /// <param name="nickname">Никнейм игрока.</param>
        public async Task DeletePlayerAsync(string nickname)
        {
            var parameters = new { p_nickname = nickname };
            await ExecuteProcAsync("DeletePlayer", parameters);
        }

        /// <summary>
        /// Получает срез игроков (порцию) для бесконечной прокрутки.
        /// </summary>
        /// <param name="sliceNumber">Номер среза (0 = первые 15).</param>
        public async Task<List<Player>> GetPlayersSliceAsync(int sliceNumber)
        {
            var parameters = new { p_slice_number = sliceNumber };
            return await QueryProcAsync<Player>("GetPlayersSlice", parameters);
        }

        /// <summary>
        /// Получает список игр, принадлежащих игроку.
        /// </summary>
        /// <param name="nickname">Никнейм игрока.</param>
        public async Task<List<Game>> GetPlayerGamesAsync(string nickname)
        {
            var parameters = new { p_nickname = nickname };
            return await QueryProcAsync<Game>("GetGamesByPlayer", parameters);
        }

        /// <summary>
        /// Добавляет игру в коллекцию игрока.
        /// </summary>
        /// <param name="nickname">Никнейм игрока.</param>
        /// <param name="gameTitle">Название игры.</param>
        public async Task AddGameToPlayerAsync(string nickname, string gameTitle)
        {
            var parameters = new
            {
                p_nickname = nickname,
                p_title = gameTitle
            };
            await ExecuteProcAsync("AddGameToPlayer", parameters);
        }

        /// <summary>
        /// Удаляет игру из коллекции игрока.
        /// </summary>
        /// <param name="nickname">Никнейм игрока.</param>
        /// <param name="gameTitle">Название игры.</param>
        public async Task RemoveGameFromPlayerAsync(string nickname, string gameTitle)
        {
            var parameters = new
            {
                p_nickname = nickname,
                p_title = gameTitle
            };
            await ExecuteProcAsync("RemoveGameFromPlayer", parameters);
        }
    }
}