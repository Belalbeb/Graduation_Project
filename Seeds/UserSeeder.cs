using Bogus;
using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Seeds
{
    public class UserSeeder
    {
        public static async Task SeedUsers(
           UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Any())
                return;

            var faker = new Faker();

            for (int i = 0; i < 10; i++)
            {
                var email = faker.Internet.Email();

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                await userManager.CreateAsync(user, "Test123#");
            }
        }
        
        }
    }

