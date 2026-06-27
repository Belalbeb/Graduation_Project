using Graduation_Project.Models;
using Stripe;

namespace Graduation_Project.Dtos
{
    public class JobPostingDto
    {
        public int JobPostingCount { get; set; }
        public int ActiveJobPostedCount { get; set; }
        public int ApplicantCount { get; set; }
       public ICollection<JobDetails> ?JobDetails { set; get; }
    }
   public class JobDetails
    {
        public Guid JobId { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        
        public List<string> JobType { get; set; }
        public bool IsActive { get; set; }
        public DateTime PostedAt { get; set; }
        public int ApplicationCount { get; set; }
        public string JobStatus { get; set; }

    }
}
