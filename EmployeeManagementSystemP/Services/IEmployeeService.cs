using System.Linq.Expressions;
using EmployeeManagementSystemP.DTOs;
using EmployeeManagementSystemP.Models;

public interface IEmployeeService
{
    //Task<Employee> OAddEmployeeAsync(string name, DateTime date, string email, string address, int gender, int countryId, int stateId, int cityId);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<int> AddEmployeeAsync(EmployeePostDto dto);
    Task<List<EmployeeDto>> GetAllEmployeesAsync();

    Task<object> GetAllEmployeesAsync(int pageNumber, int pageSize);

    Task BulkUpdateEmployeesAsync(List<EmployeeDto> employees);
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);
    Task<List<EmployeeDto>> FilterEmployeesAsync(Func<Employee, bool> predicate);
    
    Task<bool> UpdateEmployeeAsync(int id, EmployeeDto updatedEmployee);
    Task<bool> SoftDeleteEmployeeAsync(int id);
   




    // Task<List<Employee>> GetAllEmployeesAsync();
    // Task<List<EmployeeDto>> GetAllEmployeesAsync();
    /*Task<bool> UpdateEmployeeAsync(
            int employeeId, string name, DateTime date, string email, string address,
            Gender gender, int countryId, int stateId, int cityId);*/


}
