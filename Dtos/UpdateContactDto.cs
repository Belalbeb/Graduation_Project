using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class UpdateContactDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string? Email { get; set; }

        
        [RegularExpression(
            @"^(\+20|0)?1[0125][0-9]{8}$",
            ErrorMessage = "Invalid Egyptian phone number")]
        public string? Phone { get; set; }
        

        [StringLength(200, ErrorMessage = "Address can't exceed 200 characters")]
        public string? Address { get; set; }

       
        public string? Linkedin { get; set; }
      
        public string? Behance { get; set; }
      
        public string? Dribbble { get; set; }


 
        public string? Github { get; set; }


        public string? Facebook { get; set; }

        public string? Portfolio { get; set; }
    }
}