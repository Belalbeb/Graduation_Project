namespace Graduation_Project.Dtos
{
    public class InterviewCompanyResponseDto
    {
        public Guid InterviewId { get; set; }
        public string ApplicantName { get; set; }
        public string ImageUrl { get; set; }
        public string PositionTitle { get; set; }
        public string Email { get; set; }
        public string ResumePath { get; set; }
        public string InterviewStatus { get; set; }
        public DateOnly InterviewDate { get; set; }
        public string InterviewerName { get; set; }
        public string InterviewerPosition { get; set; }
        public string InterviewLink { get; set; }

    }
}
