using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;

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
                var userRoles = await userManager.GetRolesAsync(user);
                if (userRoles.Any())
                    continue;

                if (user.Email == UserSeeder.AdminEmail)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                    continue;
                }

                int chance = random.Next(1, 101);

                if (chance <= 30)
                    await userManager.AddToRoleAsync(user, Roles.Company);
                else
                    await userManager.AddToRoleAsync(user, Roles.Applicant);
            }
        }
    }
}
