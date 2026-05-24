namespace Graduation_Project.Dtos.Company.Interview
{
    public class CompanyInterviewDetailsDto
    {
        public Guid InterviewId { get; set; }
        public Guid ApplicantId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? InterviewerName { get; set; }
        public string? InterviewType { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }
    }
}
