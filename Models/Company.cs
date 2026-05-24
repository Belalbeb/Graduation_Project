namespace Graduation_Project.Models
{
    public class Company
    {
        public Guid CompanyID { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string WebsiteURL { get; set; } = string.Empty;
        public string HeadquarterAddress { get; set; } = string.Empty;

        public string? LogoUrl { get; set; }
        public string? CoverLogoUrl { get; set; }

        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? CompanySize { get; set; }
        public int? FoundedYear { get; set; }
        public string? ProfileBio { get; set; }
        public string? Description { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Linkedin { get ; set ;}

        // FK to Identity User
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        // Relations
        public ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
    }
}
