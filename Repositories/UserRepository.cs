
using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(ApplicationDbContext dbContext,UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<int> GetUserCountAsync()
        {
            return await userManager.Users.CountAsync();
        }
    }
}
