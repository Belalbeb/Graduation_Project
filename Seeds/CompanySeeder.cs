using Bogus;
using Microsoft.AspNetCore.Identity;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class CompanySeeder
    {
        public static async Task Seed(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            if (context.Companies.Any())
                return;

            var companyUsers = await userManager.GetUsersInRoleAsync(Roles.Company);

            if (!companyUsers.Any())
                return;

            var faker = new Faker<Company>()
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.HeadquarterAddress, f => f.Address.FullAddress())
                .RuleFor(c => c.Location, f => f.Address.City())
                .RuleFor(c => c.Industry,
                    f => f.PickRandom(new[]
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
                    }))
                .RuleFor(c => c.WebsiteURL, f => f.Internet.Url())
                .RuleFor(c => c.LogoUrl,
                    f => $"https://logo.clearbit.com/{f.Internet.DomainName()}")
                .RuleFor(c => c.UserId,
                    f => f.PickRandom(companyUsers).Id);

            var data = faker.Generate(companyUsers.Count);

            context.Companies.AddRange(data);
            await context.SaveChangesAsync();
        }
    }
}