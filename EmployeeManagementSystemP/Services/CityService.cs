using EmployeeManagementSystemP.Data;
using EmployeeManagementSystemP.DTOs;
using EmployeeManagementSystemP.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemP.Services
{
    public class CityService:ICityService
    {
        private readonly AppDbContext _context;

        public CityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<City> AddCityAsync(string cityName, int stateId)
        {
            var city = new City
            {
                CityName = cityName,
                StateId = stateId
            };

            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return city;
        }

        public async Task DeleteCityAsync(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
                throw new Exception($"City with ID {id} not found.");

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
        }
        public async Task AddCitiesBulkAsync(List<CityInputModel> citiesInput)
        {
            var cities = new List<City>();
            foreach (var city in citiesInput)
            {
                cities.Add(new City
                {
                    CityName = city.CityName,
                    StateId = city.StateId
                });
            }

            _context.Cities.AddRange(cities);
            await _context.SaveChangesAsync();
        }
    }
}



