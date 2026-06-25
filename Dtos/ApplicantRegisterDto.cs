using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class ApplicantRegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        // Applicant profile fields]
    
   
        [Required]
        public string Location { get; set; }
        [Required]
        public string Industry { get; set; }
    }
}
