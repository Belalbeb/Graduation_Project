using Bogus;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Seeds
{
    public class ApplicationSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Applications.Any())
                return;

            var applicants = await context.Applicants.ToListAsync();
            var jobs = await context.JobPostings.Where(j => j.IsActive).ToListAsync();
            var resumes = await context.Resumes.ToListAsync();

            if (!applicants.Any() || !jobs.Any())
                return;

            var faker = new Faker();
            var applications = new List<Application>();

            foreach (var applicant in applicants)
            {
                var applyCount = faker.Random.Int(2, Math.Min(7, jobs.Count));
                var pickedJobs = faker.PickRandom(jobs, applyCount).DistinctBy(j => j.JobID);
                var resume = resumes.FirstOrDefault(r => r.ApplicantID == applicant.ApplicantID);

                foreach (var job in pickedJobs)
                {
                    // Ensure applied date is after job posted date
                    var minDate = job.PostedDate > DateTime.UtcNow.AddYears(-1)
                        ? job.PostedDate
                        : DateTime.UtcNow.AddYears(-1);

                    var appliedDate = faker.Date.Between(minDate, DateTime.UtcNow);

                    applications.Add(new Application
                    {
                        ApplicationStatus = faker.PickRandom<ApplicationStatus>(),
                        AppliedDate = appliedDate,
                        CoverLetter = faker.Random.Bool(0.75f) ? faker.Lorem.Paragraphs(2) : null,
                        MatchScore = faker.Random.Int(40, 98),
                        ApplicantID = applicant.ApplicantID,
                        JobPostingID = job.JobID,
                        ResumeID = resume?.ResumeID
                    });
                }
            }

            context.Applications.AddRange(applications);
            await context.SaveChangesAsync();

            // Keep JobMetric.ApplicationCount in sync with actual applications.
            var applicationCounts = await context.Applications
                .GroupBy(a => a.JobPostingID)
                .Select(g => new { JobId = g.Key, Count = g.Count() })
                .ToListAsync();

            var metrics = await context.JobMetrics.ToListAsync();

            foreach (var metric in metrics)
            {
                var count = applicationCounts.FirstOrDefault(x => x.JobId == metric.JobID)?.Count ?? 0;
                metric.ApplicationCount = count;
                metric.LastUpdated = DateTime.UtcNow;
            }

            await context.SaveChangesAsync();
        }
    }
}
