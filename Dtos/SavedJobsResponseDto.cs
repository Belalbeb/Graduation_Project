namespace Graduation_Project.Dtos
{
    public class SavedJobsResponseDto
    {
        public string CompanyLogoUrl { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobRequirement { get; set; }
        public string SalaryRange { get; set; }
        public List<string> JobType { get; set; }
        public string TimeAgo { get; set; }
        public DateTime SavedAt { get; set; }
    }
}
