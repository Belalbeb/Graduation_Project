using Graduation_Project.Models;

public class Company
{
    public Guid CompanyID { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;

    public int MinEmployees { get; set; }
    public int MaxEmployees { get; set; }

    public string? WebsiteURL { get; set; }
    public string? HeadquarterAddress { get; set; }
    public string? Location { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverLogoUrl { get; set; }
    public string? Country { get; set; }
    public string? CompanySize { get; set; }
    public int? FoundedYear { get; set; }
    public string? ProfileBio { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Linkedin { get; set; }
    public string? Instagram { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }

    public CompanyStatus Status { get; set; } = CompanyStatus.Pending;
    public bool IsBlocked { get; set; } = false;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
}

public enum CompanyStatus
{
    Pending = 0,
    Verified = 1,
    Blocked = 2
}