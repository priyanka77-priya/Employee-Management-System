using EmployeeManagementSystemP.DTOs;

namespace EmployeeManagementSystemP.Services
{
    public interface IAuthService
    {
        string Authenticate(LoginDto login);
    }
}
