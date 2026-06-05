namespace Graduation_Project.Models
{
    public class Company
    {
        public Guid CompanyID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Industry { get; set; }
        public string ?WebsiteURL { get; set; }
        public string ?HeadquarterAddress { get; set; }
        public string Location { get; set; }
        public string LogoUrl { get; set; } = DefaultImage.CompanyImage;
        public int MinEmployees { get; set; }
        public int MaxEmployees { get; set; }

        // FK to Identity User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Relations
        public ICollection<JobPosting> JobPostings { get; set; }
    }
}
