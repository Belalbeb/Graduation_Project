namespace Graduation_Project.Dtos.Admin.Company
{
    public class AdminCompanyListDto
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public int TotalJobs { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }
        public string SubscriptionPlan { get; set; } = string.Empty;
    }
}
