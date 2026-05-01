namespace Graduation_Project.Dtos
{
    public class SettingsProfileDto
    {
        public string FullName { get; set; } = string.Empty ;
        public string JobTitle { get; set; } = string.Empty ;
        public string AboutMe { get; set; } = string.Empty ;
        public string? ProfilePicUrl { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public string? ResumeUrl { get; set; }
    }
}
