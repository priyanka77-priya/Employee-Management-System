using EmployeeManagementSystemP.Models;

namespace EmployeeManagementSystemP.Services
{
    public interface ILanguageService
    {
       
            Task<Language> AddLanguageAsync(string languageName);
             Task<List<Language>> GetAllLanguagesAsync();

    }
}
