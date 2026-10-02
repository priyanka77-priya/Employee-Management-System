using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemP.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; } // PK

        [Required, MaxLength(100)]
        public string ? Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, EmailAddress]
        public string ? EmailAddress { get; set; }

        [Required, MaxLength(250)]
        public string ? Address { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public int CountryId { get; set; } // FK
        public int StateId { get; set; } // FK
        public int CityId { get; set; } // FK

        // Navigation properties

        public Country ? Country { get; set; }
        public State ? State { get; set; }
        public City ? City { get; set; }

        public bool isDeleted   { get; set; }

        public virtual ICollection<EmployeeLanguage> EmployeeLanguages { get; set; } 
    }
}
