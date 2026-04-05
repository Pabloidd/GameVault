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
    /// Репозиторий для работы с издателями.
    /// </summary>
    public class PublishersRepository : AbstractRepository, IPublishersRepository
    {
        public PublishersRepository(IOptions<MariaDbOptions> options)
            : base(options) { }

        /// <summary>
        /// Получает список всех издателей.
        /// </summary>
        public async Task<List<Publisher>> GetAllPublishersAsync()
        {
            return await QueryProcAsync<Publisher>("GetAllPublishers");
        }

        /// <summary>
        /// Получает издателей по стране.
        /// </summary>
        /// <param name="country">Название страны.</param>
        public async Task<List<Publisher>> GetPublishersByCountryAsync(string country)
        {
            var parameters = new { p_country = country };
            return await QueryProcAsync<Publisher>("GetPublishersByCountry", parameters);
        }

        /// <summary>
        /// Создаёт нового издателя.
        /// </summary>
        /// <param name="company">Название компании.</param>
        /// <param name="country">Страна.</param>
        public async Task CreatePublisherAsync(string company, string country)
        {
            var parameters = new
            {
                p_company = company,
                p_country = country
            };
            await ExecuteProcAsync("CreatePublisher", parameters);
        }

        /// <summary>
        /// Обновляет страну издателя.
        /// </summary>
        /// <param name="company">Название компании (идентификатор).</param>
        /// <param name="newCountry">Новая страна.</param>
        public async Task UpdatePublisherAsync(string company, string newCountry)
        {
            var parameters = new
            {
                p_company = company,
                p_new_country = newCountry
            };
            await ExecuteProcAsync("UpdatePublisher", parameters);
        }

        /// <summary>
        /// Удаляет издателя.
        /// </summary>
        /// <param name="company">Название компании.</param>
        public async Task DeletePublisherAsync(string company)
        {
            var parameters = new { p_company = company };
            await ExecuteProcAsync("DeletePublisher", parameters);
        }

        /// <summary>
        /// Получает срез издателей (порцию) для бесконечной прокрутки.
        /// </summary>
        /// <param name="sliceNumber">Номер среза (0 = первые 15).</param>
        public async Task<List<Publisher>> GetPublishersSliceAsync(int sliceNumber)
        {
            var parameters = new { p_slice_number = sliceNumber };
            return await QueryProcAsync<Publisher>("GetPublishersSlice", parameters);
        }
    }
}