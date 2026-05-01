using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class ExperienceResponseDto
    {
        public int ExperienceID { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ApplicantID { get; set; }
    }
}
