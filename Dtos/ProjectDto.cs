using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class ProjectDto
    {
        [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters")]
        public string? Description { get; set; }

        
        public string? ProjectUrl { get; set; }

      
        public string? GithubRepoUrl { get; set; }

     
        public IFormFile? Image { get; set; }
    }
}
