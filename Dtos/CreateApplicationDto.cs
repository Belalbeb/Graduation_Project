using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class CreateApplicationDto
    {
        [Required]
        public Guid JobPostingID { get; set; }

        [Required]
 
        public Guid? ResumeID { get; set; }     

    }
}
