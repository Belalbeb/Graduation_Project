namespace Graduation_Project.Dtos
{
    public class SavedJobsResponseDto
    {
        public string CompanyLogoUrl { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public Guid jobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobRequirement { get; set; }
        public decimal minSalary { get; set; }
        public decimal maxSalary { get; set; }
        public List<string> JobType { get; set; }
        public DateTime TimeAgo { get; set; }
        public bool isApplied { get; set; }
       
    }
}
