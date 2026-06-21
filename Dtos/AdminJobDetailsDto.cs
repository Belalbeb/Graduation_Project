namespace Graduation_Project.Dtos
{
    public class AdminJobDetailsDto
    {
        public Guid JobID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Responsibility { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }

        public string JobCategory { get; set; }
        public string Location { get; set; }
        public string PostedAgo { get; set; }
        public string Status { get; set; }

        public List<string> Skills { get; set; } = new();
        public List<string> JobTypes { get; set; } = new();
        public List<string> WorkApproaches { get; set; } = new();

        public int ApplicationsCount { get; set; }

        // Company block
        public string CompanyName { get; set; }
        public string? CompanyLogoUrl { get; set; }
        public string ?CompanySize { get; set; }

        public string CompanyIndustry { get; set; }
        public Guid CompanyID { get; set; }
        public List<CandidateDto> candidates { get; set; } = new();

    }

    public class CandidateDto
    {
        public Guid ApplicationId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public string? AvatarUrl { get; set; }
        public string ApplicationStatus { get; set; }
        public string AppliedAgo { get; set; }   // "Applied 3H ago"
        public int MatchPercentage { get; set; }
        public string? CvUrl { get; set; }
        public Guid ApplicantId { get; set; }
    }
}