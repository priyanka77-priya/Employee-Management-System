using EmployeeManagementSystemP.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemP.Services

{
    public interface ICountryService
    {
    Task<List<Country>> GetAllCountriesAsync();
    Task AddCountriesBulkAsync(List<string> countryNames);
    Task<Country>AddCountryAsync(string countryName);
        Task<bool> UpdateCountryIdAsync(int oldId, int newId);
        Task<Country?> GetCountryByIdAsync(int id);
    }
}