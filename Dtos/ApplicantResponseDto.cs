using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class ApplicantDashboardResponseDto
    {
        // Statistics Cards
        public StatisticsDto Statistics { get; set; }

        // Bar Chart - Applications and Interviews per month
        public List<MonthlyStatDto> MonthlyStats { get; set; }

        // Recently Applied Jobs Table
        public List<RecentApplicationDto> RecentApplications { get; set; }
    }

    public class StatisticsDto
    {
        public int AppliedJobsCount { get; set; }       // 24
        public int SavedJobsCount { get; set; }          // 12
        public int UpcomingInterviewsCount { get; set; } // 5
        public int ProfileViewsCount { get; set; }       // 7
    }

    public class MonthlyStatDto
    {
        public string Month { get; set; }           // "Jan", "Feb", ...
        public int ApplicationsCount { get; set; }  // Blue bar
        public int InterviewsCount { get; set; }    // Green bar
    }

    public class RecentApplicationDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string JobTitle { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
