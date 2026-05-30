namespace Graduation_Project.Dtos.Admin.Applicant
{
    public class AdminUserDetailsDto
    {
        // Basic Info
        public Guid ApplicantId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }

        // Contact Info
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }

        // Statistics
        public int ApplicationsCount { get; set; }
        public int SavedJobsCount { get; set; }
        public int InterviewsCount { get; set; }
        public int ProjectsCount { get; set; }

        // Skills
        public List<string> Skills { get; set; } = new();

        // Social Links & CV
        public string? CvUrl { get; set; }
        public string? Portfolio { get; set; }
        public string? Facebook { get; set; }
        public string? Linkedin { get; set; }
        public string? Github { get; set; }
    }
}
