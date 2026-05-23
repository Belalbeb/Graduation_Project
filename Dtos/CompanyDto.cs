using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
   
    public class CompanyDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        // Company profile fields

        [Required]
        public string Industry { get; set; }
        //[Required]
        //public string WebsiteURL { get; set; }
        [Required]
        public string Location { get; set; }
       
    }
}
