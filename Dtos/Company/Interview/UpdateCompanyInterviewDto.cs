namespace Graduation_Project.Dtos.Company.Interview
{
    public class UpdateCompanyInterviewDto
    {
        public string? Status { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewType { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }
    }
}
