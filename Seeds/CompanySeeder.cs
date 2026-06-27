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

            var freePlan = await SeedDependencies.GetFreePlanAsync(context);
            var faker = new Faker();

            var industries = new[]
            {
                "Software", "Finance", "Healthcare", "Education",
                "E-commerce", "Automotive", "Real Estate", "Gaming",
                "Marketing", "AI / Data Science"
            };

            var companySizes = new[] { "1-10", "11-50", "51-200", "201-500", "500+" };

            var companies = new List<Company>();
            var subscriptions = new List<CompanySubscription>();

            foreach (var user in companyUsers)
            {
                var name = faker.Company.CompanyName();
                var slug = name.ToLower().Replace(" ", "-").Replace(",", "").Replace(".", "");
                var industry = faker.PickRandom(industries);
                var minEmployees = faker.Random.Int(10, 500);
                var createdAt = faker.Date.Past(2);

                // Build a plausible domain from the company name
                var domain = $"{slug.Split('-')[0]}{faker.PickRandom(new[] { ".com", ".io", ".co" })}";

                var company = new Company
                {
                    Name = name,
                    Industry = industry,
                    MinEmployees = minEmployees,
                    MaxEmployees = minEmployees + faker.Random.Int(50, 2000),
                    WebsiteURL = $"https://www.{domain}",
                    HeadquarterAddress = faker.Address.FullAddress(),
                    Location = faker.Address.City(),
                    Country = faker.Address.Country(),
                    CompanySize = faker.PickRandom(companySizes),
                    FoundedYear = faker.Random.Int(1980, 2022),
                    LogoUrl = $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(name)}&background=random&size=150",
                    CoverLogoUrl = $"https://picsum.photos/seed/{user.Id}/1200/400",
                    ProfileBio = faker.Company.CatchPhrase(),
                    Description = faker.Lorem.Paragraphs(2),
                    PhoneNumber = faker.Phone.PhoneNumber("+1-###-###-####"),
                    Linkedin = $"https://linkedin.com/company/{slug}",
                    Instagram = $"https://instagram.com/{slug}",
                    Facebook = $"https://facebook.com/{slug}",
                    Twitter = $"https://twitter.com/{slug}",
                    Status = CompanyStatus.Active,
                    UserId = user.Id
                };

                companies.Add(company);
            }

            await context.Companies.AddRangeAsync(companies);
            await context.SaveChangesAsync();

            // Mirror AuthController: every new company gets the Free plan immediately.
            foreach (var company in companies)
            {
                subscriptions.Add(
                    SeedDependencies.CreateFreeSubscription(
                        company.CompanyID,
                        freePlan.Id,
                        faker.Date.Past(1)));
            }

            context.companySubscriptions.AddRange(subscriptions);
            await context.SaveChangesAsync();
        }
    }
}
