namespace Graduation_Project.Dtos.Company.Interview
{
    public class CompanyInterviewListDto
    {
        public Guid InterviewId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? InterviewerName { get; set; }
    }
}
