namespace Graduation_Project.Models
{
    public class SavedJobs
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid JobPostingId { get; set; }
        public JobPosting JobPosting { get; set; }

        public Guid ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
    

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}
