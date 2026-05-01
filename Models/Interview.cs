namespace Graduation_Project.Models
{
    public class Interview
    {
      
        public int InterviewId { get; set; }


        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }

  
        public int JobPostingId { get; set; }
        public JobPosting JobPosting { get; set; }


        public DateTime ScheduledAt { get; set; }

        public InterviewStatus Status { get; set; } = InterviewStatus.Upcoming ;

        public string? InterviewerName { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }
    }

    public enum InterviewStatus
    {
        Upcoming,
        Completed,
        Cancelled
    }
}
