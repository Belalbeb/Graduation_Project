namespace Graduation_Project.Dtos.Company.Interview
{
    public class CompanyInterviewListDto
    {
        public Guid InterviewId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public DateOnly ScheduledDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        
        public string Status { get; set; } = string.Empty;
    
    }
}
