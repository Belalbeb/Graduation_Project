using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface IWebsiteSettingsService
    {
         Task<WebsiteSettingsDto?> GetAsync();
        Task<bool> UpdateAsync(UpdateWebsiteSettingsDto dto);
    }
}
