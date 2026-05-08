using Bogus;
using Graduation_Project.Models;
using System;

namespace Graduation_Project.Seeds
{
    public class SavedJobsSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.SavedJobs.Any())
                return;

            var applicants = context.Applicants.ToList();
            var jobs = context.JobPostings.ToList();

            if (!applicants.Any() || !jobs.Any())
                return;

            var faker = new Faker<SavedJobs>()
                .RuleFor(s => s.ApplicantId,
                    f => f.PickRandom(applicants).ApplicantID)

                .RuleFor(s => s.JobPostingId,
                    f => f.PickRandom(jobs).JobID)

                .RuleFor(s => s.SavedAt,
                    f => f.Date.Past(1));

            var savedJobs = faker.Generate(20);

            context.SavedJobs.AddRange(savedJobs);
            await context.SaveChangesAsync();
        }
    }
}