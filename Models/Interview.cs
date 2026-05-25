namespace Graduation_Project.Models
{
    public class Interview
    {
        public Guid InterviewId { get; set; } = Guid.NewGuid();

        public Guid ApplicantId { get; set; }
        public Applicant Applicant { get; set; }

        public Guid JobPostingId { get; set; }
        public JobPosting JobPosting { get; set; }

        public DateTime ScheduledAt { get; set; }

        public InterviewStatus Status { get; set; } = InterviewStatus.Upcoming;

        public string? InterviewerName { get; set; }
        public string? InterviewerPosition { get; set; }
        public string? MeetingLink { get; set; }

        public string? InterviewType { get ; set ;}
        public string? Notes { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public enum InterviewStatus
    {
        Upcoming,
        Completed,
        Cancelled,
        Pending
    }
}
