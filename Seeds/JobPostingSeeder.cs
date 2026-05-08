using Bogus;
using Graduation_Project.Models;
using System;

namespace Graduation_Project.Seeds
{
    public class JobPostingSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.JobPostings.Any())
                return;

            var companies = context.Companies.ToList();

            var faker = new Faker<JobPosting>()
                .RuleFor(j => j.Title, f => f.Name.JobTitle())
                .RuleFor(j => j.Description, f => f.Lorem.Paragraph())
                .RuleFor(j => j.Responsibility, f => f.Lorem.Sentence())
                .RuleFor(j => j.MinSalary, f => f.Random.Decimal(5000, 10000))
                .RuleFor(j => j.MaxSalary, f => f.Random.Decimal(10000, 20000))
                .RuleFor(j => j.Location, f => f.Address.City())
                .RuleFor(j => j.JobCategory,
    f => f.PickRandom(new[]
    {
        "Software Development",
        "Data Science",
        "Cyber Security",
        "Web Development",
        "Mobile Development",
        "UI/UX Design",
        "DevOps",
        "Cloud Computing",
        "AI / Machine Learning",
        "QA / Testing",
        "IT Support",
        "Networking"
    }))
                .RuleFor(j => j.CompanyID, f => f.PickRandom(companies).CompanyID);

            var data = faker.Generate(10);

            context.JobPostings.AddRange(data);
            await context.SaveChangesAsync();
        }
    }
}