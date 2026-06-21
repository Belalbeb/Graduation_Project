using Graduation_Project.Models;

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
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string InterviewerName { get; set; }
        public string InterviewType { get; set; }

        public string Notes { get; set; }
        public string InterviewLink { get; set; }

    }
}
