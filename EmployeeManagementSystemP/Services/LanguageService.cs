using EmployeeManagementSystemP.Data;
using EmployeeManagementSystemP.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemP.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _context;

        public LanguageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Language> AddLanguageAsync(string languageName)
        {
            if (string.IsNullOrWhiteSpace(languageName))
                throw new ArgumentException("Language name is required.");

            var existingLanguage = await _context.Languages
    .Where(l => l.LanguageName == languageName)
    .FirstOrDefaultAsync();

            if (existingLanguage != null)
                throw new InvalidOperationException("Language already exists.");


            var language = new Language
            {
                LanguageName = languageName
            };

            _context.Languages.Add(language);
            await _context.SaveChangesAsync();

            return language;
        }
        public async Task<List<Language>> GetAllLanguagesAsync()
        {
            return await _context.Languages.ToListAsync();
        }
    }
}
