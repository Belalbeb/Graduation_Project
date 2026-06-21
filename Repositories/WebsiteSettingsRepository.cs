using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Graduation_Project.Repositories
{
    public class WebsiteSettingsRepository : IWebsiteSettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public WebsiteSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WebsiteSettings?> GetAsync()
        {
            return await _context.WebsiteSettings.FirstOrDefaultAsync();
        }

        public async Task AddAsync(WebsiteSettings settings)
        {
            await _context.WebsiteSettings.AddAsync(settings);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
