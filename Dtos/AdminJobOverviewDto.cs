namespace Graduation_Project.Dtos
{
    public class AdminJobOverviewDto
    {
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int PendingJobs { get; set; }
        public int RejectedJobs { get; set; }
        public List<JobView> jobs { get; set; }
    }
    public class JobView
    {
        public Guid JobId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string Category { get; set; }
        public List<string> Type { get; set; }
        public string Status { get; set; }
        public int Applications { get; set; }
        public DateTime PostedDate { get; set; }

    }
}
