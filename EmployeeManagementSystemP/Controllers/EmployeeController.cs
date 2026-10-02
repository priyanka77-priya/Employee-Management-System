using EmployeeManagementSystemP.DTOs;
using EmployeeManagementSystemP.Models;
using EmployeeManagementSystemP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystemP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly ILanguageService _languageService;
        private readonly IEmployeeLanguageService _employeeLanguageService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IAuthService _authService;

        public EmployeeController(
            IEmployeeService employeeService,
            ICountryService countryService,
            IStateService stateService,
            ICityService cityService,
            ILanguageService languageService,
            IEmployeeLanguageService employeeLanguageService,
            ILogger<EmployeeController> logger,
            IAuthService authService)
        {
            _employeeService = employeeService;
            _countryService = countryService;
            _stateService = stateService;
            _cityService = cityService;
            _languageService = languageService;
            _employeeLanguageService = employeeLanguageService;
            _logger = logger;
            _authService = authService;
        }

        
        [HttpPost("AddCountry")]
        public async Task<IActionResult> AddCountry(string countryName)
        {
            try
            {
                _logger.LogInformation("AddCountry called with CountryName: {CountryName}", countryName);

                var country = await _countryService.AddCountryAsync(countryName);

                if (country == null)
                {
                    _logger.LogWarning("Country '{CountryName}' already exists.", countryName);
                    return Conflict($"Country '{countryName}' already exists.");
                }

                _logger.LogInformation("Country added successfully: {Country}", country);
                return StatusCode(201, country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddCountry");
                return StatusCode(500, "An error occurred while adding country.");
            }
        }


        [HttpPost("AddCountriesBulk")]
        public async Task<IActionResult> AddCountriesBulk([FromBody] List<string> countryNames)
        {
            try
            {
                _logger.LogInformation("AddCountriesBulk called with {Count} countries", countryNames.Count);
                await _countryService.AddCountriesBulkAsync(countryNames);
                _logger.LogInformation("Countries added successfully");
                return Ok(new { message = "Countries added successfully", count = countryNames.Count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddCountriesBulk");
                return StatusCode(500, "An error occurred while adding countries in bulk.");
            }
        }

        [HttpGet("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                _logger.LogInformation("GetAllCountries called");
                var countries = await _countryService.GetAllCountriesAsync();
                if (countries == null || countries.Count == 0)
                {
                    _logger.LogWarning("No countries found");
                    return NoContent(); // 204 No Content
                }
                return Ok(countries);//200 ok
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllCountries");
                return StatusCode(500, "An error occurred while retrieving countries.");
            }
        }

        [HttpGet("GetCountryById/{id}")]
        public async Task<IActionResult> GetCountryById(int id)
        {
            try
            {
                _logger.LogInformation("GetCountryById called with ID: {Id}", id);
                var country = await _countryService.GetCountryByIdAsync(id);

                if (country == null)
                {
                    _logger.LogWarning("Country with ID {Id} not found", id);
                    return NotFound($"Country with ID {id} not found.");
                }

                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetCountryById");
                return StatusCode(500, "An error occurred while retrieving country.");
            }
        }

        [HttpPut("UpdateCountryId/{oldId}/{newId}")]
        public async Task<IActionResult> UpdateCountryId(int oldId, int newId)
        {
            try
            {
                _logger.LogInformation("UpdateCountryId called: oldId={OldId}, newId={NewId}", oldId, newId);
                var result = await _countryService.UpdateCountryIdAsync(oldId, newId);

                if (!result)
                {
                    _logger.LogWarning("Country with ID {OldId} not found", oldId);
                    return NotFound($"Country with ID {oldId} not found.");
                }

                _logger.LogInformation("Country ID updated from {OldId} to {NewId}", oldId, newId);
                return Ok($"Country ID updated from {oldId} to {newId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdateCountryId");
                return StatusCode(500, "An error occurred while updating country ID.");
            }
        }

        [HttpPost("AddState")]
        public async Task<IActionResult> AddState(string stateName, int countryId)
        {
            try
            {
                _logger.LogInformation("AddState called with StateName: {StateName}, CountryId: {CountryId}", stateName, countryId);
                var state = await _stateService.AddStateAsync(stateName, countryId);
                _logger.LogInformation("State added successfully: {State}", state);
                return StatusCode(201, state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddState");
                return StatusCode(500, "An error occurred while adding state.");
            }
        }

        [HttpPost("AddStatesBulk")]
        public async Task<IActionResult> AddStatesBulk([FromBody] List<StateInputModel> states)
        {
            try
            {
                _logger.LogInformation("AddStatesBulk called with {Count} states", states.Count);
                await _stateService.AddStatesBulkAsync(states);
                _logger.LogInformation("States added successfully");
                return Ok(new { message = "States added successfully", count = states.Count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddStatesBulk");
                return StatusCode(500, "An error occurred while adding states in bulk.");
            }
        }

        [HttpGet("GetAllStates")]
        public async Task<IActionResult> GetAllStates()
        {
            try
            {
                _logger.LogInformation("GetAllStates called");
                var states = await _stateService.GetAllStatesAsync();
                return Ok(states);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllStates");
                return StatusCode(500, "An error occurred while retrieving states.");
            }
        }

        [HttpDelete("DeleteState/{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            try
            {
                _logger.LogInformation("DeleteState called with ID: {Id}", id);
                var result = await _stateService.DeleteStateByIdAsync(id);

                if (!result)
                {
                    _logger.LogWarning("State with ID {Id} not found", id);
                    return NotFound($"State with ID {id} not found.");
                }

                _logger.LogInformation("State with ID {Id} deleted successfully", id);
                return Ok($"State with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteState");
                return StatusCode(500, "An error occurred while deleting the state.");
            }
        }


        [HttpPost("AddCity")]
        public async Task<IActionResult> AddCity(string cityName, int stateId)
        {
            try
            {
                _logger.LogInformation("AddCity called with CityName: {CityName}, StateId: {StateId}", cityName, stateId);
                var city = await _cityService.AddCityAsync(cityName, stateId);
                _logger.LogInformation("City added successfully: {City}", city);
                return StatusCode(201, city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddCity");
                return StatusCode(500, "An error occurred while adding city.");
            }
        }

        [HttpPost("AddCitiesBulk")]
        public async Task<IActionResult> AddCitiesBulk([FromBody] List<CityInputModel> cities)
        {
            try
            {
                _logger.LogInformation("AddCitiesBulk called with {Count} cities", cities.Count);
                await _cityService.AddCitiesBulkAsync(cities);
                _logger.LogInformation("Cities added successfully");
                return Ok(new { message = "Cities added successfully", count = cities.Count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddCitiesBulk");
                return StatusCode(500, "An error occurred while adding cities in bulk.");
            }
        }

        [HttpDelete("DeleteCity/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                _logger.LogInformation("DeleteCity called with ID: {Id}", id);
                await _cityService.DeleteCityAsync(id);
                _logger.LogInformation("City with ID {Id} deleted successfully", id);
                return Ok(new { message = $"City with id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteCity");
                return StatusCode(500, "An error occurred while deleting the city.");
            }
        }

        [HttpGet("GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                _logger.LogInformation("GetAllCities called");
                var cities = await _cityService.GetAllCitiesAsync();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllCities");
                return StatusCode(500, "An error occurred while retrieving cities.");
            }
        }
        [HttpPost("AddLanguage")]
        public async Task<IActionResult> AddLanguage(string languageName)
        {
            try
            {
                _logger.LogInformation("AddLanguage called with LanguageName: {LanguageName}", languageName);
                var language = await _languageService.AddLanguageAsync(languageName);
                _logger.LogInformation("Language added successfully: {Language}", language);
                return StatusCode(201, language);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddLanguage");
                return StatusCode(500, "An error occurred while adding the language.");
            }
        }

        [HttpGet("GetAllLanguages")]
        public async Task<IActionResult> GetAllLanguagesAsync()
        {
            try
            {
                _logger.LogInformation("GetAllLanguages called");
                var languages = await _languageService.GetAllLanguagesAsync();
                return Ok(languages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllLanguages");
                return StatusCode(500, "An error occurred while retrieving languages.");
            }
        }

        [HttpPost("AssignLanguageToEmployees")]
        public async Task<IActionResult> AssignLanguage(int employeeId, int languageId, string proficiencyLevel)
        {
            try
            {
                _logger.LogInformation("AssignLanguage called with EmployeeId: {EmployeeId}, LanguageId: {LanguageId}, ProficiencyLevel: {ProficiencyLevel}", employeeId, languageId, proficiencyLevel);
                var result = await _employeeLanguageService.AssignLanguageAsync(employeeId, languageId, proficiencyLevel);

                if (result)
                {
                    _logger.LogInformation("Language assigned successfully");
                    return StatusCode(201, "Language assigned successfully.");
                }

                _logger.LogWarning("Language assignment failed");
                return BadRequest("Assignment failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AssignLanguage");
                return StatusCode(500, "An error occurred while assigning the language.");
            }
        }

        [HttpGet("GetAllEmployeeLanguages")]
        public async Task<IActionResult> GetAllEmployeeLanguages()
        {
            try
            {
                _logger.LogInformation("GetAllEmployeeLanguages called");
                var result = await _employeeLanguageService.GetAllEmployeeLanguagesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllEmployeeLanguages");
                return StatusCode(500, "An error occurred while retrieving employee languages.");
            }
        }

        [HttpPut("UpdateProficiency")]
        public async Task<IActionResult> UpdateProficiency(int employeeId, int languageId, string newProficiencyLevel)
        {
            try
            {
                _logger.LogInformation("UpdateProficiency called with EmployeeId: {EmployeeId}, LanguageId: {LanguageId}, NewProficiencyLevel: {ProficiencyLevel}", employeeId, languageId, newProficiencyLevel);

                if (string.IsNullOrWhiteSpace(newProficiencyLevel))
                {
                    _logger.LogWarning("New proficiency level is required");
                    return BadRequest("New proficiency level is required.");
                }

                var updatedEmpLang = await _employeeLanguageService.UpdateProficiencyAsync(employeeId, languageId, newProficiencyLevel);

                if (updatedEmpLang == null)
                {
                    _logger.LogWarning("Language assignment not found for EmployeeId: {EmployeeId}", employeeId);
                    return NotFound("Language assignment not found for the given employee.");
                }

                _logger.LogInformation("Proficiency level updated successfully");
                return Ok(updatedEmpLang);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdateProficiency");
                return StatusCode(500, "An error occurred while updating proficiency level.");
            }
        }

        [Authorize]
        [HttpGet("getall/{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAllEmployees(int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("GetAllEmployees called with PageNumber: {PageNumber}, PageSize: {PageSize}", pageNumber, pageSize);

                if (pageNumber <= 0 || pageSize <= 0)
                {
                    _logger.LogWarning("Invalid page parameters: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
                    return BadRequest("PageNumber and PageSize must be greater than 0.");
                }

                var result = await _employeeService.GetAllEmployeesAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllEmployees");
                return StatusCode(500, "An error occurred while retrieving employees.");
            }
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeePostDto dto)
        {
            try
            {
                _logger.LogInformation("AddEmployee called");

                if (dto == null)
                {
                    _logger.LogWarning("Invalid data received");
                    return BadRequest("Invalid data.");
                }

                var employeeId = await _employeeService.AddEmployeeAsync(dto);
                _logger.LogInformation("Employee added with ID: {EmployeeId}", employeeId);

                return StatusCode(201, employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddEmployee");
                return StatusCode(500, "An error occurred while adding the employee.");
            }
        }
        



        

        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto updatedEmployee)
        {
            if (id != updatedEmployee.EmployeeId)
                return BadRequest("Employee ID mismatch");

            var result = await _employeeService.UpdateEmployeeAsync(id, updatedEmployee);

            if (!result)
                return NotFound("Employee not found or update failed");

            return Ok("Employee updated successfully");
        }









        /* [HttpPut("UpdateEmployee")]
         public async Task<IActionResult> UpdateEmployee(
             int employeeId, string name, DateTime date, string email,
             string address, Gender gender, int countryId, int stateId, int cityId)

         {
             try
             {
                 _logger.LogInformation("UpdateEmployee called for EmployeeId: {EmployeeId}", employeeId);
                 bool result = await _employeeService.UpdateEmployeeAsync(
                     employeeId, name, date, email, address, gender, countryId, stateId, cityId);

                 if (!result)
                 {
                     _logger.LogWarning("Employee with ID {EmployeeId} not found", employeeId);
                     return NotFound("Employee not found");
                 }

                 _logger.LogInformation("Employee updated successfully: {EmployeeId}", employeeId);
                 return Ok("Employee updated successfully");
             }
             catch (Exception ex)
             {
                 _logger.LogError(ex, "Error occurred in UpdateEmployee");
                 return StatusCode(500, "An error occurred while updating the employee.");
             }
         }*/

        /* [HttpGet("GetAllEmployees")]
         public async Task<IActionResult> GetAllEmployees()
         {
             _logger.LogInformation("GetAllEmployees called");
             var employees = await _employeeService.GetAllEmployeesAsync();
             return Ok(employees);
         }
        */
        [Authorize]
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                _logger.LogInformation("GetAllEmployees called");
                
                List<EmployeeDto> employees = await _employeeService.GetAllEmployeesAsync();
                if (employees == null || employees.Count == 0)
                {
                    _logger.LogWarning("No employees found");
                    return NoContent(); // 204
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllEmployees");
                return StatusCode(500, "An error occurred while retrieving employees.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                _logger.LogInformation("DeleteEmployee called for EmployeeId: {Id}", id);
                var result = await _employeeService.DeleteEmployeeAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Employee with ID {Id} not found or already deleted", id);
                    return NotFound(new { message = $"Employee with ID {id} not found or already deleted." });
                }

                _logger.LogInformation("Employee with ID {Id} deleted successfully", id);
                return Ok(new { message = "Employee deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteEmployee");
                return StatusCode(500, "An error occurred while deleting the employee.");
            }
        }

        [HttpGet("GetEmployeeByID/{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);

                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found.");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetEmployeeById");
                return StatusCode(500, "An error occurred while retrieving the employee.");
            }
        }



        [HttpGet("ActiveEmployee")]
        public async Task<IActionResult> ActiveEmployees()
        {
            try
            {
                _logger.LogInformation("Filtering employees: started.");

                var result = await _employeeService.FilterEmployeesAsync(e => !e.isDeleted);
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No active employees found.");
                    return NotFound("No active employees found."); // 404
                }

                _logger.LogInformation("Filtering employees: completed successfully with {Count} records.", result.Count());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering employees.");
                return StatusCode(500, "An error occurred while fetching employee data.");
            }
        }

        [HttpGet("InActiveEmployee")]
        public async Task<IActionResult> InActiveEmployees()
        {
            try
            {
                _logger.LogInformation("Filtering employees: started.");

                var result = await _employeeService.FilterEmployeesAsync(e => e.isDeleted);
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No inactive employees found.");
                    return NotFound("No inactive employees found.");
                }

                _logger.LogInformation("Filtering employees: completed successfully with {Count} records.", result.Count());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering employees.");
                return StatusCode(500, "An error occurred while fetching employee data.");
            }
        }




        [HttpPut("Bulk-UpdateEmployee")]
        public async Task<IActionResult> BulkUpdateEmployees([FromBody] List<EmployeeDto> employees)
        {
            if (employees == null || employees.Count == 0)
            {
                _logger.LogWarning("No employee data provided for bulk update.");
                return BadRequest("No employee data provided for bulk update.");
            }

            try
            {
                _logger.LogInformation("BulkUpdateEmployees called with {Count} employees", employees.Count);
                await _employeeService.BulkUpdateEmployeesAsync(employees);
                _logger.LogInformation("Bulk update successful.");
                return Ok(new { message = "Bulk update successful." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during bulk update.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteEmployee(int id)
        {
            try
            {
                var success = await _employeeService.SoftDeleteEmployeeAsync(id);
                if (!success)
                {
                    return NotFound($"Employee with ID {id} not found or already deleted.");
                }

                return Ok($"Employee with ID {id} soft deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while soft deleting employee with ID {EmployeeId}", id);
                return StatusCode(500, "An error occurred while soft deleting the employee.");
            }
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            var token = _authService.Authenticate(login);
            if (token == null)
                return Unauthorized();

            return Ok(new { token });
        }

    }




    /* [HttpPost("AddEmployee")]
     public async Task<IActionResult> OAddEmployee(
         string name,
         DateTime date,
         string email,
         string address,
         int gender,
         int countryId,
         int stateId,
         int cityId)
     {
         try
         {
             var employee = await _employeeService.AddEmployeeAsync(name, date, email, address, gender, countryId, stateId, cityId);
             return StatusCode(201, employee);
         }
         catch (ArgumentException ex)
         {
             return BadRequest(ex.Message);
         }
     }*/


    /*[HttpGet("GetAllEmployees")]
    public async Task<IActionResult> OGetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    */
}


