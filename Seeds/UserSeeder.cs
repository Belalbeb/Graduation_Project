using Bogus;
using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Seeds
{
    public class UserSeeder
    {
        public const string AdminEmail = "hima@gmail.com";
        public const string DefaultPassword = "Test123#";
        public const string AdminPassword = "Test123#";

        public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Any())
                return;

            var faker = new Faker();

            // Seed admin
            var admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
             
            };
            await userManager.CreateAsync(admin, AdminPassword);

            // Seed 15 regular users (was 10) for richer relational data
            for (int i = 0; i < 100; i++)
            {
                var firstName = faker.Name.FirstName();
                var lastName = faker.Name.LastName();
                var email = faker.Internet.Email(firstName, lastName);

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                  
                };

                await userManager.CreateAsync(user, DefaultPassword);
            }
        }
    }
}
