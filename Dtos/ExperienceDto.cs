using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class ExperienceDto
    {
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public JobType JobType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
