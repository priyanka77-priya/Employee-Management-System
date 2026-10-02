using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemP.Models
{
    public class EmployeeLanguage //it act as a bridge for employee and language a
    
    // one employee knows many languages 
    // one language is known by many employees
    //So we are using the employyelangauge table to connect both employee and language 
    


    {
        [Required]
        public int EmployeeId {  get; set; } //Fk
        [Required]
        public int LanguageId  {  get; set; } //Fk
        //navigation property
        public Employee? Employee   { get; set; }
        public Language ? Language { get; set; }
        [Required, MaxLength(50)]
        public string ? ProficiencyLevel { get; set; }
       
    }
}
