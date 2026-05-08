namespace Graduation_Project.Models
{
    public class Project
    {
        public Guid ProjectID { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ProjectUrl { get; set; }
        public string? GithubRepoUrl { get; set; }
        public string? ImageUrl { get; set; }  // Cloudinary

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Foreign Key
        public Guid ApplicantID { get; set; }
        public Applicant Applicant { get; set; } = null!;
    }
}
