using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class CreateJobDto
    {
        public JobBasicDataDto JobBasicData { get; set; }
        public JobDetailsResponseDto JobDetails { get; set; }
    }

    public class JobBasicDataDto
    {
        [Required]
        public List<string> EmploymentType { get; set; }
        [Required]
        public string JobCategory { get; set; }
        [Required]
        [MinLength(3)]
        public string JobTitle { get; set; }
        [Required]
        public string Location { get; set; }
        public decimal SalaryMax { get; set; }
        public decimal SalaryMin { get; set; }
        [Required]
        public List<string> WorkApproach { get; set; }
    }

    public class JobDetailsResponseDto
    {
        [MinLength(20)]
        [Required]
        public string JobDescription { get; set; }
        [MinLength(20)]
        [Required]
        public string Responsibilities { get; set; }
        public List<string> Skills { get; set; }
    }
}
