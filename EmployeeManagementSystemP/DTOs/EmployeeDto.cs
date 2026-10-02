
using EmployeeManagementSystemP.DTOs;
    public class EmployeeDto 
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string EmailAddress { get; set; }
    public string Address { get; set; }
    public int Gender { get; set; }
    public int CountryId { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
    public string CountryName { get; set; }
    public string StateName { get; set; }
    public string CityName { get; set; }
    public List<LanguageProficiencyDto> Languages { get; set; }  // important
}