using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameVault.DTO;
using GameVault.Models;
using GameVault.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : BaseController
    {
        private readonly ICountryRepository _countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public Task<ActionResult<List<Country>>> GetAllCountries()
        {
            return ExecuteListAsync(() => _countryRepository.GetAllCountriesAsync());
        }

        [HttpPost]
        public async Task<ActionResult> CreateCountry([FromBody] string countryName)
        {
            var error = RequireString(countryName, "Название страны");
            if (error != null) return error;

            return await ExecuteAsync(
                () => _countryRepository.CreateCountryAsync(countryName),
                $"Страна '{countryName}' успешно создана"
            );
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCountry([FromBody] UpdateCountryRequest request)
        {
            var error1 = RequireString(request.OldCountryName, "Старое название страны");
            if (error1 != null) return error1;

            var error2 = RequireString(request.NewCountryName, "Новое название страны");
            if (error2 != null) return error2;

            return await ExecuteAsync(
                () => _countryRepository.UpdateCountryAsync(request.OldCountryName, request.NewCountryName),
                $"Страна '{request.OldCountryName}' обновлена на '{request.NewCountryName}'"
            );
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCountry([FromBody] string countryName)
        {
            var error = RequireString(countryName, "Название страны");
            if (error != null) return error;

            return await ExecuteAsync(
                () => _countryRepository.DeleteCountryAsync(countryName),
                $"Страна '{countryName}' удалена"
            );
        }
    }
}