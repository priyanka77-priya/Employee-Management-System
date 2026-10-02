using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemP.Models
{
    public class Language 
    {
        public int LanguageId  {  get; set; }
        [Required,MaxLength(100)]//annotations used for validation
        public string ? LanguageName {  get; set; }
        public virtual ICollection<EmployeeLanguage> EmployeeLanguages { get; set; }//one to many relations

    }
}
