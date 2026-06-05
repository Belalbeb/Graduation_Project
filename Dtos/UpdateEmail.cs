using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class UpdateEmail
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
