using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameVault.Models;
using GameVault.Repositories;
using GameVault.Options;  
using Microsoft.Extensions.Options;
using System.Data;
using Dapper;

namespace GameVault.Repositories
{
    public class CountryRepository : AbstractRepository, ICountryRepository
    {
        public CountryRepository(IOptions<MariaDbOptions> options)
            : base(options.Value.ConnectionString){}
        

        public async Task CreateCountryAsync(string countryName)
        {
            string sql = $"CALL CreateCountry({countryName})";
            await ExecuteWithConnectionAsync(sql);
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            string sql = "CALL GetAllCountries()";
            return await QueryWithConnectionAsync<Country>(sql);
        }

        public async Task DeleteCountryAsync(string countryName)
        {
            string sql = $"CALL DeleteCountry({countryName})";
            await ExecuteWithConnectionAsync(sql);
        }

        public async Task UpdateCountryAsync(string oldCountryName, string newCountryName)
        {
            string sql = $"CALL UpdateCountry({oldCountryName}, {newCountryName})";
            await ExecuteWithConnectionAsync(sql);
        }
    }
}