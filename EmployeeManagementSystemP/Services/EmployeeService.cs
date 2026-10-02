using EmployeeManagementSystemP.Data;
using EmployeeManagementSystemP.DTOs;
using EmployeeManagementSystemP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemP.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

       

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || employee.isDeleted)
            {
                return false;
            }

            // Soft delete
            employee.isDeleted = true;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return true;
        }

       





        public async Task<bool> UpdateEmployeeAsync(int id, EmployeeDto updatedEmployee)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (id != updatedEmployee.EmployeeId)
                    return false;

                var existingEmployee = await _context.Employees
                    .Include(e => e.EmployeeLanguages)///navigationproperty
                    .FirstOrDefaultAsync(e => e.EmployeeId == id && !e.isDeleted);

                if (existingEmployee == null)
                    return false;

                existingEmployee.Name = updatedEmployee.Name;
                existingEmployee.Date = updatedEmployee.Date;
                existingEmployee.EmailAddress = updatedEmployee.EmailAddress;
                existingEmployee.Address = updatedEmployee.Address;
                existingEmployee.Gender = (Gender)updatedEmployee.Gender;
                existingEmployee.CountryId = updatedEmployee.CountryId;
                existingEmployee.StateId = updatedEmployee.StateId;
                existingEmployee.CityId = updatedEmployee.CityId;

                _context.EmployeesLanguages.RemoveRange(existingEmployee.EmployeeLanguages);

                if (updatedEmployee.Languages != null && updatedEmployee.Languages.Any())
                {
                    existingEmployee.EmployeeLanguages = updatedEmployee.Languages.Select(lang => new EmployeeLanguage
                    {
                        EmployeeId = id,
                        LanguageId = lang.LanguageId,
                        ProficiencyLevel = lang.Proficiency
                    }).ToList();
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }







        
        public async Task<object> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Employees.CountAsync(e => !e.isDeleted); 
            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => !e.isDeleted)
                .Include(e => e.Country)
                .Include(e => e.State)
                .Include(e => e.City)
                .Include(e => e.EmployeeLanguages)
                    .ThenInclude(el => el.Language)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    Date = e.Date,
                    EmailAddress = e.EmailAddress,
                    Address = e.Address,
                    Gender = (int)e.Gender,

                    CountryId = e.CountryId,
                    StateId = e.StateId,
                    CityId = e.CityId,
                    CountryName = e.Country != null ? e.Country.CountryName : null,
                    StateName = e.State != null ? e.State.StateName : null,
                    CityName = e.City != null ? e.City.CityName : null,

                    Languages = e.EmployeeLanguages.Select(el => new LanguageProficiencyDto
                    {
                        LanguageName = el.Language != null ? el.Language.LanguageName : null,
                        Proficiency = el.ProficiencyLevel
                    }).ToList()
                })
                .ToListAsync();

            return new
            {
                totalRecords,
                pageNumber,
                pageSize,
                data = employees
            };
        }





        public async Task<int> AddEmployeeAsync(EmployeePostDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = new Employee
                {
                    Name = dto.Name,
                    Date = dto.Date,
                    EmailAddress = dto.EmailAddress,
                    Address = dto.Address,
                    Gender = (Gender)dto.Gender,
                    CountryId = dto.CountryId,
                    StateId = dto.StateId,
                    CityId = dto.CityId
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                if (dto.Languages != null && dto.Languages.Any())
                {
                    var empLanguages = dto.Languages.Select(lang => new EmployeeLanguage
                    {
                        EmployeeId = employee.EmployeeId,
                        LanguageId = lang.LanguageId,
                        ProficiencyLevel = lang.Proficiency
                    }).ToList();

                    await _context.EmployeesLanguages.AddRangeAsync(empLanguages);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return employee.EmployeeId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .Where(e => !e.isDeleted)
                .Include(e => e.Country)
                .Include(e => e.State)
                .Include(e => e.City)
                .Include(e => e.EmployeeLanguages)
                    .ThenInclude(el => el.Language)
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    Date = e.Date,
                    EmailAddress = e.EmailAddress,
                    Address = e.Address,
                    Gender = (int)e.Gender,

                    CountryId = e.CountryId,
                    StateId = e.StateId,
                    CityId = e.CityId,
                    CountryName = e.Country.CountryName,
                    StateName = e.State.StateName,
                    CityName = e.City.CityName,

                    Languages = e.EmployeeLanguages
                        .Select(el => new LanguageProficiencyDto
                        {
                            LanguageId = el.LanguageId,
                            LanguageName = el.Language.LanguageName,
                            Proficiency = el.ProficiencyLevel
                        })
                        .ToList()
                })
                .ToListAsync();
        }


        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Country)
                .Include(e => e.State)
                .Include(e => e.City)
                .Include(e => e.EmployeeLanguages)
                    .ThenInclude(el => el.Language)
                    
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
            

            if (employee == null)
                return null;

            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Date = employee.Date,
                EmailAddress = employee.EmailAddress,
                Address = employee.Address,
                Gender = (int)employee.Gender,
                CountryId = employee.CountryId,
                StateId = employee.StateId,
                CityId = employee.CityId,
                CountryName = employee.Country?.CountryName,
                StateName = employee.State?.StateName,
                CityName = employee.City?.CityName,
                Languages = employee.EmployeeLanguages.Select(el => new LanguageProficiencyDto
                {
                    LanguageId = el.LanguageId,
                    LanguageName = el.Language.LanguageName,
                    Proficiency = el.ProficiencyLevel
                }).ToList()
            };
        }

        public async Task<List<EmployeeDto>> FilterEmployeesAsync(Func<Employee, bool> predicate)
        {
            var employees = await Task.Run(() =>
                _context.Employees
                    .Include(e => e.Country)
                    .Include(e => e.State) 
                    .Include(e => e.City)
                    .Include(e => e.EmployeeLanguages)
                        .ThenInclude(el => el.Language)
                    .AsEnumerable() // Convert to in-memory for Func<> support
                    .Where(predicate) //filtering starts here
                    .Select(e => new EmployeeDto
                    {
                        EmployeeId = e.EmployeeId,
                        Name = e.Name,
                        Date = e.Date,
                        EmailAddress = e.EmailAddress,
                        Address = e.Address,
                        Gender = (int)e.Gender,

                        CountryId = e.CountryId,
                        StateId = e.StateId,
                        CityId = e.CityId,
                        CountryName = e.Country?.CountryName,
                        StateName = e.State?.StateName,
                        CityName = e.City?.CityName,

                        Languages = e.EmployeeLanguages.Select(el => new LanguageProficiencyDto
                        {
                            LanguageId = el.LanguageId,
                            LanguageName = el.Language?.LanguageName,
                            Proficiency = el.ProficiencyLevel
                        }).ToList()
                    })
                    .ToList()
            );

            return employees;
        }
        public async Task BulkUpdateEmployeesAsync(List<EmployeeDto> employees)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var empDto in employees)
                {
                    var existingEmp = await _context.Employees
                        .Include(e => e.EmployeeLanguages)
                        .FirstOrDefaultAsync(e => e.EmployeeId == empDto.EmployeeId && !e.isDeleted);

                    if (existingEmp != null)
                    {
                        existingEmp.Name = empDto.Name;
                        existingEmp.Date = empDto.Date;
                        existingEmp.EmailAddress = empDto.EmailAddress;
                        existingEmp.Address = empDto.Address;
                        existingEmp.Gender = (Gender)empDto.Gender;
                        existingEmp.CountryId = empDto.CountryId;
                        existingEmp.StateId = empDto.StateId;
                        existingEmp.CityId = empDto.CityId;

                        // Update Language Proficiencies
                        existingEmp.EmployeeLanguages.Clear();

                        if (empDto.Languages != null)
                        {
                            foreach (var lang in empDto.Languages)
                            {
                                existingEmp.EmployeeLanguages.Add(new EmployeeLanguage
                                {
                                    EmployeeId = empDto.EmployeeId,
                                    LanguageId = lang.LanguageId,
                                    ProficiencyLevel = lang.Proficiency
                                });
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> SoftDeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null || employee.isDeleted)
            {
                return false;
            }

            employee.isDeleted = true;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return true;
        }

      


        /*  public async Task<int> AddEmployeeAsync(EmployeePostDto dto)
         {
             var employee = new Employee
             {
                 Name = dto.Name,
                 Date = dto.Date,
                 EmailAddress = dto.EmailAddress,
                 Address = dto.Address,
                 Gender = (Gender)dto.Gender,
                 CountryId = dto.CountryId,
                 StateId = dto.StateId,
                 CityId = dto.CityId
             };

             await _context.Employees.AddAsync(employee);
             await _context.SaveChangesAsync();

             if (dto.Languages != null && dto.Languages.Any())
             {
                 var empLanguages = dto.Languages.Select(lang => new EmployeeLanguage
                 {
                     EmployeeId = employee.EmployeeId,
                     LanguageId = lang.LanguageId,
                     ProficiencyLevel = lang.Proficiency
                 }).ToList();

                 await _context.EmployeesLanguages.AddRangeAsync(empLanguages);
                 await _context.SaveChangesAsync();
             }

             return employee.EmployeeId;
         }*/





        /* public async Task BulkUpdateEmployeesAsync(List<EmployeeDto> employees)
         {
             foreach (var empDto in employees)
             {
                 var existingEmp = await _context.Employees
                     .Include(e => e.EmployeeLanguages)
                     .FirstOrDefaultAsync(e => e.EmployeeId == empDto.EmployeeId && !e.isDeleted);

                 if (existingEmp != null)
                 {
                     existingEmp.Name = empDto.Name;
                     existingEmp.Date = empDto.Date;
                     existingEmp.EmailAddress = empDto.EmailAddress;
                     existingEmp.Address = empDto.Address;
                     existingEmp.Gender = (Gender)empDto.Gender;
                     existingEmp.CountryId = empDto.CountryId;
                     existingEmp.StateId = empDto.StateId;
                     existingEmp.CityId = empDto.CityId;

                     // Update Language Proficiencies
                     if (empDto.Languages != null)
                     {
                         existingEmp.EmployeeLanguages.Clear();  // remove old links

                         foreach (var lang in empDto.Languages)
                         {
                             existingEmp.EmployeeLanguages.Add(new EmployeeLanguage
                             {
                                 EmployeeId = empDto.EmployeeId,
                                 LanguageId = lang.LanguageId,
                                 ProficiencyLevel = lang.Proficiency
                             });
                         }
                     }


                 }


                 await _context.SaveChangesAsync();

             }
         }

         /*   public async Task<bool> UpdateEmployeeAsync(
         int employeeId, string name, DateTime date, string email, string address,
         Gender gender, int countryId, int stateId, int cityId)
          {
              var employee = await _context.Employees.FindAsync(employeeId);


              if (employee == null)
                  return false;

              employee.Name = name;
              employee.Date = date;
              employee.EmailAddress = email;
              employee.Address = address;
              employee.Gender = gender;
              employee.CountryId = countryId;
              employee.StateId = stateId;
              employee.CityId = cityId;

              await _context.SaveChangesAsync();
              return true;
          }*/

        /*  public async Task<object> GetAllEmployeesAsync(int pageNumber, int pageSize)
          {
              var totalRecords = await _context.Employees.CountAsync();

              var employees = await _context.Employees
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

              return new
              {
                  TotalRecords = totalRecords,
                  PageNumber = pageNumber,
                  PageSize = pageSize,
                  Data = employees
              };
          }
        */
        /* public async Task<Employee> OAddEmployeeAsync(string name, DateTime date, string email, string address, int gender, int countryId, int stateId, int cityId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            if (date == default)
                throw new ArgumentException("Valid date is required");

            var employee = new Employee
            {
                Name = name,
                Date = date,
                EmailAddress = email,
                Address = address,
                Gender = (Gender)gender,
                CountryId = countryId,
                StateId = stateId,
                CityId = cityId,
                isDeleted = false
            };


            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
            /*var employeeWithRelations = await _context.Employees
        .Include(e => e.Country)
        .Include(e => e.State)
        .Include(e => e.City)
        .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

            return employeeWithRelations!;*/

        /*  public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
          {
              var employees = await _context.Employees
                  .Where(e => !e.isDeleted)
                  .Include(e => e.EmployeeLanguages)
                      .ThenInclude(el => el.Language)
                  .ToListAsync();

              var employeeDtos = employees.Select(employee => new EmployeeDto
              {
                  EmployeeId = employee.EmployeeId,
                  Name = employee.Name,
                  Date = employee.Date,
                  EmailAddress = employee.EmailAddress,
                  Address = employee.Address,
                  Gender = (int)employee.Gender,
                  CountryId = employee.CountryId,
                  StateId = employee.StateId,
                  CityId = employee.CityId,
                  Languages = employee.EmployeeLanguages.Select(el => new LanguageProficiencyDto
                  {
                      LanguageId = el.LanguageId,
                      LanguageName = el.Language?.LanguageName,
                      Proficiency = el.ProficiencyLevel
                  }).ToList()
              }).ToList();

              return employeeDtos;
          }*/

    }
}






