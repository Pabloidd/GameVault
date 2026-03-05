using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameVault.Models;
using GameVault.Repositories;
using Microsoft.AspNetCore.Mvc;
using GameVault.RequestModels;

namespace GameVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : BaseController
    {
        private readonly IPublishersRepository _publishersRepository;

        public PublishersController(IPublishersRepository publishersRepository)
        {
            _publishersRepository = publishersRepository;
        }

        [HttpGet]
        public Task<ActionResult<List<Publisher>>> GetAllPublishers()
        {
            return ExecuteListAsync(() => _publishersRepository.GetAllPublishersAsync());
        }

        [HttpGet("by-country/{country}")]
        public async Task<ActionResult<List<Publisher>>> GetPublishersByCountry(string country)
        {
            var error = RequireString(country, "Название страны");
            if (error != null) return error;

            return await ExecuteListAsync(() => _publishersRepository.GetPublishersByCountryAsync(country));
        }

        [HttpPost]
        public async Task<ActionResult> CreatePublisher([FromBody] CreatePublisherRequest request)
        {
            var error1 = RequireString(request.Company, "Название компании");
            if (error1 != null) return error1;

            var error2 = RequireString(request.Country, "Страна");
            if (error2 != null) return error2;

            return await ExecuteAsync(
                () => _publishersRepository.CreatePublisherAsync(request.Company, request.Country),
                $"Издатель '{request.Company}' успешно создан"
            );
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePublisher([FromBody] UpdatePublisherRequest request)
        {
            var error1 = RequireString(request.Company, "Название компании");
            if (error1 != null) return error1;

            var error2 = RequireString(request.NewCountry, "Новая страна");
            if (error2 != null) return error2;

            return await ExecuteAsync(
                () => _publishersRepository.UpdatePublisherAsync(request.Company, request.NewCountry),
                $"Издатель '{request.Company}' успешно обновлен"
            );
        }

        [HttpDelete("{company}")]
        public async Task<ActionResult> DeletePublisher(string company)
        {
            var error = RequireString(company, "Название компании");
            if (error != null) return error;

            return await ExecuteAsync(
                () => _publishersRepository.DeletePublisherAsync(company),
                $"Издатель '{company}' успешно удален"
            );
        }

        [HttpGet("slice/{sliceNumber}")]
        public async Task<ActionResult<List<Publisher>>> GetPublishersSlice(int sliceNumber)
        {
            var error = RequireMin(sliceNumber, 0, "Номер среза");
            if (error != null) return error;

            return await ExecuteListAsync(() => _publishersRepository.GetPublishersSliceAsync(sliceNumber));
        }
    }
}