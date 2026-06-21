namespace Graduation_Project.Dtos.Company.SettingsTabs
{
    public class CompanyProfileSettingsDto
    {
        public Guid CompanyID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? CoverLogoUrl { get; set; }

        public string Industry { get; set; } = string.Empty;
        public string? Country { get; set; }
        public string? CompanySize { get; set; }
        public int? FoundedYear { get; set; }

        public string? ProfileBio { get; set; }
        public string? Description { get; set; }
    
    }
}