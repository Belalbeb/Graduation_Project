using Bogus;
using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Seeds
{
    public class CompanySeeder
    {
        public static async Task Seed(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            if (await context.Companies.AnyAsync())
                return;

            var companyUsers = await userManager.GetUsersInRoleAsync(Roles.Company);

            if (!companyUsers.Any())
                return;

            var faker = new Faker();

            // real domains that always have logos
            var domains = new[]
            {
                "google.com",
                "microsoft.com",
                "amazon.com",
                "netflix.com",
                "spotify.com",
                "uber.com",
                "airbnb.com",
                "intel.com",
                "ibm.com",
                "oracle.com",
                "adobe.com",
                "nvidia.com",
                "paypal.com"
            };

            var companies = companyUsers.Select(user =>
            {
                var domain = faker.PickRandom(domains);

                return new Company
                {
                    Name = faker.Company.CompanyName(),

                    HeadquarterAddress = faker.Address.FullAddress(),

                    Location = faker.Address.City(),

                    Industry = faker.PickRandom(new[]
                    {
                        "Software",
                        "Finance",
                        "Healthcare",
                        "Education",
                        "E-commerce",
                        "Automotive",
                        "Real Estate",
                        "Gaming",
                        "Marketing",
                        "AI / Data Science"
                    }),

                    WebsiteURL = $"https://{domain}",

                    // working logo source (returns real logos)
                    LogoUrl = $"https://geticon.dev/?url={domain}",

                    UserId = user.Id
                };
            }).ToList();

            await context.Companies.AddRangeAsync(companies);

            await context.SaveChangesAsync();
        }
    }
}