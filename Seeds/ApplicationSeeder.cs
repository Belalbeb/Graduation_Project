using Bogus;
using Graduation_Project.Models;
using System;

namespace Graduation_Project.Seeds
{
    public class ApplicationSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Applications.Any())
                return;

            var applicants = context.Applicants.ToList();
            var jobs = context.JobPostings.ToList();

            var faker = new Faker<Application>()
                .RuleFor(a => a.ApplicationStatus, f => f.PickRandom<ApplicationStatus>())
                .RuleFor(a => a.AppliedDate, f => f.Date.Past())
                .RuleFor(a => a.CoverLetter, f => f.Lorem.Paragraph())
                .RuleFor(a => a.ApplicantID, f => f.PickRandom(applicants).ApplicantID)
                .RuleFor(a => a.JobPostingID, f => f.PickRandom(jobs).JobID);

            var data = faker.Generate(10);

            context.Applications.AddRange(data);
            await context.SaveChangesAsync();
        }
    }
}