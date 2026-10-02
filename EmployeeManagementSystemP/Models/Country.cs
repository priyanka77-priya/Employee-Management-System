using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemP.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required, MaxLength(100)]
        public string ? CountryName { get; set; }

        public virtual ICollection<State> States { get; set; } 
        public virtual ICollection<Employee> Employees { get; set; } 
    }

}
