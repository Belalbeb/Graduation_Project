using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class InterviewResponseDto
    {
        public int InterviewId { get; set; }
        public int JobPostingId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? CompanyLogoUrl { get; set; }

        public DateTime ScheduledAt { get; set; }
        public string TimeZone { get; set; } = "EST" ;

        public InterviewStatus Status { get; set; }
        public string? InterviewerName { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }

    }

    public class InterviewStatisticsDto
    {
        public int Total { get; set; }
        public int Upcoming { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
    }
}
