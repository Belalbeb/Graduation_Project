namespace Graduation_Project.Models
{
    public class WebsiteSettings
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? YoutubeUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TwitterUrl { get; set; }

        public string? ContactEmail { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
