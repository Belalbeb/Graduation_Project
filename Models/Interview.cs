namespace Graduation_Project.Models
{
    public class Interview
    {
        public Guid InterviewId { get; set; } = Guid.NewGuid();

        public Guid ApplicantId { get; set; }
        public Applicant Applicant { get; set; }

        public Guid JobPostingId { get; set; }
        public JobPosting JobPosting { get; set; }

        
        public DateOnly InterviewDate { get; set; }

     
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public InterviewStatus Status { get; set; } = InterviewStatus.Upcoming;

        
        public InterviewType interviewType { get; set; } = InterviewType.Technical;

        public string InterviewerName { get; set; }
        public string? InterviewerPosition { get; set; }
<<<<<<< HEAD
        public string? MeetingLink { get; set; }

        public string? InterviewType { get ; set ;}
=======
        public string MeetingLink { get; set; }
>>>>>>> eaac8262416df8044c7b7b09e0f831adee87ff0b
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

    public enum InterviewType
    {
        Technical,
        HR,
        Behavioral,
        Final
    }
}