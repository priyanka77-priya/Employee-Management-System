using EmployeeManagementSystemP.DTOs;
using EmployeeManagementSystemP.Models;

namespace EmployeeManagementSystemP.Services
{
    public interface ICityService
    {
        Task<List<City>> GetAllCitiesAsync();
        Task<City> AddCityAsync(string cityName, int stateId);
        Task DeleteCityAsync(int id);
        Task AddCitiesBulkAsync(List<CityInputModel> cities);
    }
}
