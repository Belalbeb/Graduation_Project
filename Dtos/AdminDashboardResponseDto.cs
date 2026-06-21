using static Graduation_Project.Dtos.CompanyResponseDto;

namespace Graduation_Project.Dtos
{
    public class AdminDashboardResponseDto
    {
        public int TotalUsers { get; set; }
        public int TotalCompanies { get; set; }
        public int ActiveJobPosts { get; set; }
        public int PendingJobs { get; set; }
        public int Year { get; set; } 
        public List<MonthlyJobStatsDto> MonthlyStats { get; set; }
        public List<LatestJobDto> LatestJobs { get; set; }
        public List<PendingApprovalDto> PendingApprovals { get; set; }
    }
    public class MonthlyJobStatsDto
    {
        
        public string Month { get; set; } 
        public int JobPosts { get; set; }
        public int Applications { get; set; }
    }
    public class LatestJobDto
    {
        public Guid JobId { get; set; }
        public string JobTitle { get; set; }
        public int TotalApplications { get; set; }
        public DateTime PostedAt { get; set; }
    }
    public class PendingApprovalDto
    {
        public Guid JobId { get; set; }
        public string JobTitle { get; set; }
        public string Logo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
