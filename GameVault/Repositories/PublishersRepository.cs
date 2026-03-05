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
    public class PublishersRepository : AbstractRepository, IPublishersRepository
    {
        public PublishersRepository(IOptions<MariaDbOptions> options)
            : base(options.Value.ConnectionString) { }

        public async Task<List<Publisher>> GetAllPublishersAsync()
        {
            return await QueryProcAsync<Publisher>("GetAllPublishers");
        }

        public async Task<List<Publisher>> GetPublishersByCountryAsync(string country)
        {
            var parameters = new { p_country = country };
            return await QueryProcAsync<Publisher>("GetPublishersByCountry", parameters);
        }

        public async Task CreatePublisherAsync(string company, string country)
        {
            var parameters = new 
            { 
                p_company = company,
                p_country = country 
            };
            await ExecuteProcAsync("CreatePublisher", parameters);
        }

        public async Task UpdatePublisherAsync(string company, string newCountry)
        {
            var parameters = new 
            { 
                p_company = company,
                p_new_country = newCountry 
            };
            await ExecuteProcAsync("UpdatePublisher", parameters);
        }

        public async Task DeletePublisherAsync(string company)
        {
            var parameters = new { p_company = company };
            await ExecuteProcAsync("DeletePublisher", parameters);
        }

        public async Task<List<Publisher>> GetPublishersSliceAsync(int sliceNumber)
        {
            var parameters = new { p_slice_number = sliceNumber };
            return await QueryProcAsync<Publisher>("GetPublishersSlice", parameters);
        }
    }
}