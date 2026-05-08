using Microsoft.AspNetCore.Identity;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds.Identity
{
    public class UserRoleSeeder
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager)
        {
            var users = userManager.Users.ToList();

            if (!users.Any())
                return;

            var random = new Random();

            foreach (var user in users)
            {
                // prevent duplicates
                var userRoles = await userManager.GetRolesAsync(user);
                if (userRoles.Any())
                    continue;

                int chance = random.Next(1, 101); // 1 - 100

                if (chance <= 30)
                {
                    await userManager.AddToRoleAsync(user, Roles.Company);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, Roles.Applicant);
                }
            }
        }
    }
}