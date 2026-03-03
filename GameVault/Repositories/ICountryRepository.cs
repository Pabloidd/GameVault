using GameVault.Models;

namespace GameVault.Repositories
{   
    public interface ICountryRepository
    {
        Task<List<Country>> GetAllCountriesAsync();
        
        Task CreateCountryAsync(string countryName);
        Task UpdateCountryAsync(string oldCountryName, string newCountryName);
        Task DeleteCountryAsync(string countryName);
        
    }
}