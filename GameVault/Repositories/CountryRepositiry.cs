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
    /// Репозиторий для работы со странами.
    /// </summary>
    public class CountryRepository : AbstractRepository, ICountryRepository
    {
        public CountryRepository(IOptions<MariaDbOptions> options)
            : base(options) { }

        /// <summary>
        /// Создаёт новую страну.
        /// </summary>
        /// <param name="countryName">Название страны.</param>
        public async Task CreateCountryAsync(string countryName)
        {
            var parameters = new { p_country = countryName };
            await ExecuteProcAsync("CreateCountry", parameters);
        }

        /// <summary>
        /// Получает список всех стран.
        /// </summary>
        public async Task<List<Country>> GetAllCountriesAsync()
        {
            return await QueryProcAsync<Country>("GetAllCountries");
        }

        /// <summary>
        /// Удаляет страну.
        /// </summary>
        /// <param name="countryName">Название страны.</param>
        public async Task DeleteCountryAsync(string countryName)
        {
            var parameters = new { p_country = countryName };
            await ExecuteProcAsync("DeleteCountry", parameters);
        }

        /// <summary>
        /// Обновляет название страны.
        /// </summary>
        /// <param name="oldCountryName">Старое название страны.</param>
        /// <param name="newCountryName">Новое название страны.</param>
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