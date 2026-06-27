using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class WebsiteSettingsSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.WebsiteSettings.Any())
                return;

            var settings = new WebsiteSettings
            {
                YoutubeUrl = "https://youtube.com/@jobify",
                LinkedInUrl = "https://linkedin.com/company/jobify",
                FacebookUrl = "https://facebook.com/jobify",
                InstagramUrl = "https://instagram.com/jobify",
                TwitterUrl = "https://twitter.com/jobify",
                ContactEmail = "support@jobify.com",
                PhoneNumber = "+1-800-555-0199"
            };

            context.WebsiteSettings.Add(settings);
            await context.SaveChangesAsync();
        }
    }
}
