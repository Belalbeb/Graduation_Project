using static System.Net.Mime.MediaTypeNames;

namespace Graduation_Project.Models
{
    public class Applicant
    {
        public Guid ApplicantID { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public string ?Address { get; set; }
        public string? AboutMe { get; set; }
        public string ProfilePicURL { get; set; } = DefaultImage.ApplicantImage;
        public string? CoverPhotoUrl { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Location { get; set; }
        public string? Linkedin { get; set; }
        public string? Github { get; set; }
        public string? Facebook { get; set; }
        public string? Behance { get; set; }
        public string? Dribble { get; set; }
        public string? Portfolio { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Pending;
        public bool IsBlocked { get; set; } = false;

        // FK to Identity User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Relations
        public ICollection<Experience> Experiences { get; set; }
        public ICollection<Resume> Resumes { get; set; }
        public ICollection<Application> Applications { get; set; }
        public ICollection<ApplicantSkill> ApplicantSkills { get; set; }
        public ICollection<Project> Projects { get; set; }
    }

    public enum UserStatus
    {
        Pending = 0,   // Waiting for admin approval
        Active = 1,    // Approved and active
        Blocked = 2    // Blocked by admin
    }
}
