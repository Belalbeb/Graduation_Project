using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class WebsiteSettingsService : IWebsiteSettingsService
    {
        private readonly IWebsiteSettingsRepository _repo;

        public WebsiteSettingsService(IWebsiteSettingsRepository repo)
        {
            _repo = repo;
        }

        public async Task<WebsiteSettingsDto?> GetAsync()
        {
            var settings = await _repo.GetAsync();

            if (settings == null) return null;

            return new WebsiteSettingsDto
            {
               YoutubeUrl = settings.YoutubeUrl,
                LinkedInUrl = settings.LinkedInUrl,
                FacebookUrl = settings.FacebookUrl,
                InstagramUrl = settings.InstagramUrl,
                TwitterUrl = settings.TwitterUrl
            };
        }

        public async Task<bool> UpdateAsync(UpdateWebsiteSettingsDto dto)
        {
            var settings = await _repo.GetAsync();

            if (settings == null)
            {
                settings = new WebsiteSettings();
                await _repo.AddAsync(settings);
            }

            settings.YoutubeUrl = dto.YoutubeUrl;
            settings.LinkedInUrl = dto.LinkedInUrl;
            settings.FacebookUrl = dto.FacebookUrl;
            settings.InstagramUrl = dto.InstagramUrl;
            settings.TwitterUrl = dto.TwitterUrl;

            await _repo.SaveChangesAsync();

            return true;
        }
    }
}
