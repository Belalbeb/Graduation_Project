using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class ApplicantRegisterDto
    {

        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

        // Applicant profile fields]
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Location { get; set; }
    }
}
