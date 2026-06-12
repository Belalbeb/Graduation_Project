using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Graduation_Project.Models
{
    public class JobPosting
    {
        [Key]
        public Guid JobID { get; set; } = Guid.NewGuid();

        // Basic Info
        public string Title { get; set; }
        public string Description { get; set; }
        public string Responsibility { get; set; }

        public decimal MinSalary { get; set; }
        
        public decimal MaxSalary { get; set; }

        public string JobCategory { get; set; }
        public string Location { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;
        // Employment Type (Full-time, Part-time)
       
        public List<JobType> JobTypes { get; set; } = new();

        // Work Approach (On-site, Remote, Hybrid)
        public List<WorkApproach> WorkApproaches { get; set; } = new();

        public bool IsRemote { get; set; } = false;

        // Skills (normalized)
        public ICollection<JobSkill> Skills { get; set; } = new List<JobSkill>();

        // FK
        public Guid CompanyID { get; set; }
        public Company Company { get; set; }
        public int MinExperience { get; set; }
        public int MaxExperience { get; set; }

        // Navigation
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
        public JobStatus Status { get; set; } = JobStatus.Pending;
        public JobMetric JobMetric { get; set; }
    }

    public enum JobType
    {
        FullTime = 0,
        PartTime = 1
    }

    public enum WorkApproach
    {
        OnSite = 0,
        Hybrid = 1,
        Remote = 2
    }
    public enum JobStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public class JobSkill
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public Guid JobPostingId { get; set; }
        public JobPosting JobPosting { get; set; }
    }
}
