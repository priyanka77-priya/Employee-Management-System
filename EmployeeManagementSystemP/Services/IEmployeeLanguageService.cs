using EmployeeManagementSystemP.Models;

namespace EmployeeManagementSystemP.Services
{
    public interface IEmployeeLanguageService 
    {
        Task<bool> AssignLanguageAsync(int employeeId, int languageId, string proficiencyLevel);
        Task<List<EmployeeLanguage>> GetAllEmployeeLanguagesAsync();
        Task<EmployeeLanguage> UpdateProficiencyAsync(int employeeId, int languageId, string newProficiencyLevel);
    }
}
