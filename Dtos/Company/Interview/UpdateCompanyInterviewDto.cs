namespace Graduation_Project.Dtos.Company.Interview
{
    public class UpdateCompanyInterviewDto
    {
        public string? Status { get; set; }
        public string? InterviewDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewType { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }
    }
}
