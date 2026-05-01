namespace Graduation_Project.Dtos
{
    public class PublicProfileDto
    {
        public int ApplicantID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? AboutMe { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Linkedin { get; set; }
        public string? Github { get; set; }
        public string? Facebook { get; set; }
        public string? Portfolio { get; set; }

        public List<ExperienceResponseDto> Experiences { get; set; } = new();
        public List<SkillResponseDto> Skills { get; set; } = new();
        public List<ProjectResponseDto> Projects { get; set; } = new();
    }
}
