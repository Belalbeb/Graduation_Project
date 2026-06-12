namespace Graduation_Project.Dtos.Company.Profile
{
    public class CompanyPublicProfileDto
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? CoverLogoUrl { get; set; }
        public string? Tagline { get; set; }  // ProfileBio
        public string? About { get; set; }     // Description
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string Industry { get; set; } = string.Empty;
        public string? CompanySize { get; set; }
        public int? FoundedYear { get; set; }

        public CompanySocialLinksDto? SocialLinks { get; set; }
        public CompanyStatsDto Stats { get; set; } = new();
        public List<CompanyVacancyDto> OpenVacancies { get; set; } = new();
    }

    public class CompanySocialLinksDto
    {
        public string? Facebook { get; set; }
        public string? Linkedin { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
    }

    public class CompanyStatsDto
    {
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
    }

    public class CompanyVacancyDto
    {
        public Guid JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string SalaryCurrency { get; set; } = "USD";
        public string JobType { get; set; } = string.Empty;
        public string WorkApproach { get; set; } = string.Empty;
        public DateTime PostedAt { get; set; }
    }
}
