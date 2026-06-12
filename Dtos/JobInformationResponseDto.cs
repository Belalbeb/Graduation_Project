namespace Graduation_Project.Dtos
{
    public class JobInformationResponseDto
    {
        public string Title { get; set; }
        public string JobStatus { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
        public int MinExper { get; set; }
        public int MaxExper { get; set; }
        public List<string> WorkApproaches { get; set; }
        public List<string> JobTypes { get; set; }
        public DateTime PostedDate { get; set; }
        public int ApplicantsCount { get; set; }
        public int InterviewCount { get; set; }
        public string Description { get; set; }
        public string Responsibility { get; set; }
        public List<string?> RequiredSkill { get; set; } = new List<string>();
        public string Location { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public List<ApplicantDetail> ApplicantDetails { get; set; }
        public List<InterviewDetailDto> ApplicantInterviews { get; set; }

    }
    public class ApplicantDetail
    {
        public Guid ApplicantionId { get; set; }
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; }
    }
    public class InterviewDetailDto
    {
        public Guid InterviewId { get; set; }
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly InterviewDate { get; set; }
        public string InterviewStatus { get; set; }

    }
}
