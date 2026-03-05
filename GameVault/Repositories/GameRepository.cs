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
    public class GameRepository : AbstractRepository, IGameRepository
    {
        public GameRepository(IOptions<MariaDbOptions> options) 
            : base(options.Value.ConnectionString)
        {
        }
        
        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await QueryProcAsync<Game>("GetAllGames");
        }
        
        public async Task<Game?> GetGameByTitleAsync(string title)
        {
            var parameters = new { p_title = title };
            return await QuerySingleProcAsync<Game>("GetGameByTitle", parameters);
        }
        
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
        
        public async Task DeleteGameAsync(string title)
        {
            var parameters = new { p_title = title };
            await ExecuteProcAsync("DeleteGame", parameters);
        }
        
        public async Task<List<Game>> GetGamesSliceAsync(int sliceNumber)
        {
            var parameters = new { p_slice_number = sliceNumber };
            return await QueryProcAsync<Game>("GetGamesSlice", parameters);
        }
        
        public async Task<List<Game>> GetGamesSliceByGenreAsync(int sliceNumber, string genreName)
        {
            var parameters = new 
            { 
                p_genre = genreName,
                p_slice_number = sliceNumber 
            };
            return await QueryProcAsync<Game>("GetGamesSliceByGenre", parameters);
        }
        
        public async Task<List<Game>> GetGamesSliceByPublisherAsync(int sliceNumber, string publisherName)
        {
            var parameters = new 
            { 
                p_company = publisherName,
                p_slice_number = sliceNumber 
            };
            return await QueryProcAsync<Game>("GetGamesSliceByPublisher", parameters);
        }
        
        public async Task<List<Game>> GetGamesByGenreAsync(string genreName)
        {
            var parameters = new { p_genre = genreName };
            return await QueryProcAsync<Game>("GetGamesByGenre", parameters);
        }
        
        public async Task<List<Game>> GetGamesByPublisherAsync(string publisherName)
        {
            var parameters = new { p_company = publisherName };
            return await QueryProcAsync<Game>("GetGamesByPublisher", parameters);
        }
        
        public async Task AddGenreToGameAsync(string gameName, string genreName)
        {
            var parameters = new 
            { 
                p_title = gameName,
                p_genre = genreName 
            };
            await ExecuteProcAsync("AddGenreToGame", parameters);
        }
        
        public async Task RemoveGenreFromGameAsync(string gameName, string genreName)
        {
            var parameters = new 
            { 
                p_title = gameName,
                p_genre = genreName 
            };
            await ExecuteProcAsync("RemoveGenreFromGame", parameters);
        }
        
        public async Task<List<Genre>> GetGameGenresAsync(string gameName)
        {
            var parameters = new { p_title = gameName };
            return await QueryProcAsync<Genre>("GetGameGenres", parameters);
        }
        
        public async Task<List<Player>> GetPlayersByGameAsync(string gameName)
        {
            var parameters = new { p_title = gameName };
            return await QueryProcAsync<Player>("GetPlayersByGame", parameters);
        }
    }
}