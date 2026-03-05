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
    public class PlayerRepository : AbstractRepository, IPlayerRepository
    {
        public PlayerRepository(IOptions<MariaDbOptions> options)
            : base(options.Value.ConnectionString) { }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await QueryProcAsync<Player>("GetAllPlayers");
        }

        public async Task<Player?> GetPlayerByNicknameAsync(string nickname)
        {
            var parameters = new { p_nickname = nickname };
            return await QuerySingleProcAsync<Player>("GetPlayerByNickname", parameters);
        }

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

        public async Task DeletePlayerAsync(string nickname)
        {
            var parameters = new { p_nickname = nickname };
            await ExecuteProcAsync("DeletePlayer", parameters);
        }

        public async Task<List<Player>> GetPlayersSliceAsync(int sliceNumber)
        {
            var parameters = new { p_slice_number = sliceNumber };
            return await QueryProcAsync<Player>("GetPlayersSlice", parameters);
        }

        public async Task<List<Game>> GetPlayerGamesAsync(string nickname)
        {
            var parameters = new { p_nickname = nickname };
            return await QueryProcAsync<Game>("GetGamesByPlayer", parameters);
        }

        public async Task AddGameToPlayerAsync(string nickname, string gameTitle)
        {
            var parameters = new 
            { 
                p_nickname = nickname,
                p_title = gameTitle 
            };
            await ExecuteProcAsync("AddGameToPlayer", parameters);
        }

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