using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Graduation_Project.Models
{
    public class JobPosting
    {
        [Key]
        public int JobID { get; set; }

        // Basic Info
        public string Title { get; set; }
        public string Description { get; set; }
        public string Responsibility { get; set; }

        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }

        public string JobCategory { get; set; }
        public string Location { get; set; } // ✅ added

        public DateTime PostedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Employment Type (Full-time, Part-time)
        public List<JobType> JobTypes { get; set; } = new();

        // Work Approach (On-site, Remote, Hybrid)
        public List<WorkApproach> WorkApproaches { get; set; } = new();

        public bool IsRemote { get; set; } = false;

        // Skills (normalized)
        public ICollection<JobSkill> Skills { get; set; } = new List<JobSkill>();

        // FK
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        // Navigation
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
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

    public class JobSkill
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int JobPostingId { get; set; }
        public JobPosting JobPosting { get; set; }
    }
}
