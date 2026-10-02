using EmployeeManagementSystemP.DTOs;
using EmployeeManagementSystemP.Models;

namespace EmployeeManagementSystemP.Services
{
    public interface IStateService
    {
        Task<List<State>> GetAllStatesAsync();
        Task<State> AddStateAsync(string stateName, int countryId);
        Task AddStatesBulkAsync(List<StateInputModel> states);


        Task<bool> DeleteStateByIdAsync(int id);
    }
}
