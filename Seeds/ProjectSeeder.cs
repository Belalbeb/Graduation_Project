using Bogus;
using Graduation_Project.Models;
using System;

namespace Graduation_Project.Seeds
{
    public class ProjectSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Projects.Any())
                return;

            var applicants = context.Applicants.ToList();

            if (!applicants.Any())
                return;

            var faker = new Faker<Project>()
                .RuleFor(p => p.Title,
                    f => f.Commerce.ProductName())

                .RuleFor(p => p.Description,
                    f => f.Lorem.Paragraph())

                .RuleFor(p => p.ProjectUrl,
                    f => f.Internet.Url())

                .RuleFor(p => p.GithubRepoUrl,
                    f => $"https://github.com/{f.Internet.UserName()}/{f.Lorem.Word()}")

                .RuleFor(p => p.ImageUrl,
                    f => $"https://picsum.photos/seed/{Guid.NewGuid()}/600/400")

                .RuleFor(p => p.CreatedAt,
                    f => f.Date.Past(3))

                .RuleFor(p => p.UpdatedAt,
                    (f, p) => f.Random.Bool()
                        ? p.CreatedAt.AddMonths(f.Random.Int(1, 12))
                        : null)

                .RuleFor(p => p.ApplicantID,
                    f => f.PickRandom(applicants).ApplicantID);

            var projects = faker.Generate(15);

            context.Projects.AddRange(projects);
            await context.SaveChangesAsync();
        }
    }
}