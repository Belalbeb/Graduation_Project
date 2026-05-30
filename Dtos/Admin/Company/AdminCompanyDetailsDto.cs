namespace Graduation_Project.Dtos.Admin.Company
{
    public class AdminCompanyDetailsDto
    {
        // Basic Info
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? CoverLogoUrl { get; set; }

        // Contact & Info
        public string? Email { get; set; }
        public string Industry { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? CompanySize { get; set; }

        // Status
        public string Status { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }

        // Subscription
        public string SubscriptionPlan { get; set; } = string.Empty;

        // Statistics
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int TotalApplicants { get; set; }
        public int TotalInterviews { get; set; }
    }
}
