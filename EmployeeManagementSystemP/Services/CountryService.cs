using EmployeeManagementSystemP.Data;
using EmployeeManagementSystemP.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemP.Services
    
{
    public class CountryService : ICountryService
        
    {
        private readonly AppDbContext _context ; 
        public CountryService(AppDbContext context)
        {
            _context = context;
            
        }
        public async Task<List<Country>> GetAllCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }


        public async Task AddCountriesBulkAsync(List<string> countryNames)
        {
            if (countryNames == null || !countryNames.Any())
                throw new ArgumentException("Country list is empty.");

            var countries = new List<Country>();
            foreach (var name in countryNames)
            {
                countries.Add(new Country {
                    CountryName = name 
                });
            }

            _context.Countries.AddRange(countries);
            await _context.SaveChangesAsync();
        }

        /*public async Task<Country> AddCountryAsync(string countryName)
        {
            var country = new Country { CountryName = countryName };
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return country;
        }*/
        public async Task<Country> AddCountryAsync(string countryName)
        {
            string nameToCheck = countryName.ToLower();

            var existingCountry = await _context.Countries
                .FirstOrDefaultAsync(c => c.CountryName.ToLower() == nameToCheck);

            if (existingCountry != null)
            {
                return null;
            }

            var country = new Country { CountryName = countryName };
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<Country?> GetCountryByIdAsync(int id)
        {
            return await _context.Countries.FindAsync(id);
        }

        public async Task<bool> UpdateCountryIdAsync(int oldId, int newId)
        {
            var country = await _context.Countries.FindAsync(oldId);
            if (country == null)
                return false;

            country.CountryId = newId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


