using EmployeeManagementSystemP.Data;
using EmployeeManagementSystemP.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemP.Services
{
    public class EmployeeLanguageService:IEmployeeLanguageService
    {
        private readonly AppDbContext _context;

        public EmployeeLanguageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignLanguageAsync(int employeeId, int languageId, string proficiencyLevel)
        {
            if (string.IsNullOrWhiteSpace(proficiencyLevel))
                throw new ArgumentException("Proficiency level is required.");

            var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == employeeId);
            if (!employeeExists)
                throw new InvalidOperationException("Employee not found.");

            var languageExists = await _context.Languages.AnyAsync(l => l.LanguageId == languageId);
            if (!languageExists)
                throw new InvalidOperationException("Language not found.");

            var alreadyExists = await _context.EmployeesLanguages
                .AnyAsync(el => el.EmployeeId == employeeId && el.LanguageId == languageId);

            if (alreadyExists)
                throw new InvalidOperationException("Language already assigned to this employee.");

            var empLang = new EmployeeLanguage
            {
                EmployeeId = employeeId,
                LanguageId = languageId,
                ProficiencyLevel = proficiencyLevel
            };

            _context.EmployeesLanguages.Add(empLang);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<EmployeeLanguage>> GetAllEmployeeLanguagesAsync()
        {
            return await _context.EmployeesLanguages.ToListAsync();
        }


        public async Task<EmployeeLanguage> UpdateProficiencyAsync(int employeeId, int languageId, string newProficiencyLevel)
        {
            var empLang = await _context.EmployeesLanguages
                .FirstOrDefaultAsync(el => el.EmployeeId == employeeId && el.LanguageId == languageId);

            if (empLang == null)
                return null;

            empLang.ProficiencyLevel = newProficiencyLevel;

            _context.EmployeesLanguages.Update(empLang);
            await _context.SaveChangesAsync();

            return empLang;
        }






    }
}
