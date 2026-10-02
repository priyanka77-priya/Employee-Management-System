namespace EmployeeManagementSystemP.DTOs
{
    public class EmployeePostDto  
    {
         
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public List<LanguageProficiencyDto> Languages { get; set; }
    }
}

