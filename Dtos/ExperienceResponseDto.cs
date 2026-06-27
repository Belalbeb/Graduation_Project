using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class ExperienceResponseDto
    {
        public Guid ExperienceID { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string JobType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ?EndDate { get; set; }
        public Guid ApplicantID { get; set; }
    }
}
