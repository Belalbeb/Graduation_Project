using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Application
    {
        [Key]
        public Guid ApplicationID { get; set; } = Guid.NewGuid();
        public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Pending;
        public DateTime AppliedDate { get; set; }
        public string ?CoverLetter { get; set; }

        // FKs
        public Guid ApplicantID { get; set; }
        public Applicant Applicant { get; set; }

        [ForeignKey("JobPosting")]
        public Guid JobPostingID { get; set; }
        public JobPosting JobPosting { get; set; }
        public int MatchScore { get; set; }

        public Guid? ResumeID { get; set; }
        public Resume? Resume { get; set; }
    }

    public enum ApplicationStatus
    {
        Pending,
        Reviewed,
        Accepted,
        Rejected
    }
}
