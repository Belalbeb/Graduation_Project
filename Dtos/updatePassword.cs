using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class updatePassword
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
