using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Graduation_Project.Models
{
    public class JobPosting
    {
        [Key]
        public int JobID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string SalaryRange { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public JobType JobType { get; set; }//remote fulltime or partime
        public bool IsRemote { get; set; } = false;

        // FK
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        // Navigation
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<Interview> Interviews { get; set; }
        public JobMetric JobMetric { get; set; }
    }
  public enum JobType
    {
        FullTime=0,
        PartTime=1,
        Remote=2
    }
}
