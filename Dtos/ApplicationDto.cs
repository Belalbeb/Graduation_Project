using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class ApplicationDto
    {
        public Guid ApplicationId { get; set; }

        public string LogoUrl { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
      
        public string Location { get; set; }
        public DateTime AppliedOn { get; set; }
        public string ApplicationStatus { get; set; }
        public List<string> JobType { get; set; }
        public bool IsRemote { get; set; }
    }
}
