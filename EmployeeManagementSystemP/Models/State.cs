using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace EmployeeManagementSystemP.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Required, MaxLength(100)]
        public string ? StateName { get; set; }

        [Required]
        public int CountryId { get; set; } // FK
        
        public Country ?  Country { get; set; }

        public virtual ICollection<City> Cities { get; set; } 
        public virtual ICollection<Employee> Employees { get; set; } 
    }

}
