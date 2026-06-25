namespace Graduation_Project.Dtos
{
    public class JobPostingDetailsDto
    {
        public string CompanyImage { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        public string JobCategory { get; set; } = string.Empty;
        public string JobLocation { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int MinExperience { get; set; }
        public int MaxExperience { get; set; }

        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }

        public List<string> WorkApproaches { get; set; } = new List<string>();
        public List<string> JobTypes { get; set; } = new List<string>();

        public DateTime PostedDate { get; set; }

        public string Description { get; set; } = string.Empty;
        public string Responsibility { get; set; } = string.Empty;
        public List<string> RequiredSkill { get; set; } = new List<string>();

        public bool IsActive { get; set; }
        public bool IsApplied { get; set; }
        public bool IsSaved { get; set; }

    
    }
    public class SimilarJobDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyImage { get; set; } = string.Empty;

        public string JobLocation { get; set; } = string.Empty;

        public bool IsApplied { get; set; }
        public bool IsSaved { get; set; }

        public List<string> WorkApproach { get; set; } = new List<string>();
        public List<string> JobType { get; set; } = new List<string>();

        public DateTime PostedDate { get; set; }

        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
    }
}
