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
    public class CountryRepository : AbstractRepository, ICountryRepository
    {
        public CountryRepository(IOptions<MariaDbOptions> options)
            : base(options.Value.ConnectionString) { }
        
        public async Task CreateCountryAsync(string countryName)
        {
            var parameters = new { p_country = countryName };
            await ExecuteProcAsync("CreateCountry", parameters);
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            return await QueryProcAsync<Country>("GetAllCountries");
        }

        public async Task DeleteCountryAsync(string countryName)
        {
            var parameters = new { p_country = countryName };
            await ExecuteProcAsync("DeleteCountry", parameters);
        }

        public async Task UpdateCountryAsync(string oldCountryName, string newCountryName)
        {
            var parameters = new 
            { 
                p_old_country = oldCountryName,
                p_new_country = newCountryName 
            };
            await ExecuteProcAsync("UpdateCountry", parameters);
        }
    }
}