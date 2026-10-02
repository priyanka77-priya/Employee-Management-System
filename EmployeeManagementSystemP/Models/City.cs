using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManagementSystemP.Models
{
    public class City
    {
      
       
            [Key]
            public int CityId { get; set; }

            [Required, MaxLength(100)]
            public string ? CityName { get; set; }

            [Required]
            public int StateId { get; set; } // FK
       
        public State State { get; set; }

            public virtual ICollection<Employee> Employees { get; set; } 
        }
    }