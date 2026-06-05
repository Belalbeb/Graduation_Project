namespace Graduation_Project.Dtos
{
    public class CompanyResponseDto
    {
        public string CompanyName { get; set; }
        public StatisticsDto Statistics { get; set; }

        // Bar Chart - Applications and Interviews per month
        public List<MonthlyStatDto> MonthlyStats { get; set; }

        // Recently Applied Jobs Table
        public List<RecentJobPostsDto> RecentJobPosting { get; set; }

        public List<ApplicantsDto> Applicants { get; set; }


        public class StatisticsDto
    {
        public int TotalJobPosts { get; set; }       // 24
        public int ActiveJobPosts { get; set; }          // 12
        public int TotalApplicants { get; set; } // 5
        public int ScheduledInterviews{ get; set; }       // 7
    }

    public class MonthlyStatDto
    {
        public string Month { get; set; }           // "Jan", "Feb", ...
        public int ApplicantsCount { get; set; }  // Blue bar
        public int JobPostedCount { get; set; }    // Green bar
    }

    public class RecentJobPostsDto
    {
        public Guid Id { get; set; }
        public string JobTitle { get; set; }
        public string TotalApplication { get; set; }
        
        public DateTime PostedAt { get; set; }
    }
    public class ApplicantsDto
        {
            public Guid ApplicantId { get; set; }
            public string ImageUrl { get; set; }

            public  string ApplicantName { get; set; }
            public Guid JobId { get; set; }
            public string JobAppliedFor { get; set; }
            public DateTime AppliedAt { get; set; }
        }
}
}

