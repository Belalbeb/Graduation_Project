using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IWebsiteSettingsRepository
    {
        Task<WebsiteSettings?> GetAsync();
        Task AddAsync(WebsiteSettings settings);
        Task SaveChangesAsync();
    }
}
