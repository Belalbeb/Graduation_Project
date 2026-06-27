using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class JobMetricSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.JobMetrics.Any())
                return;

            var jobs = context.JobPostings.ToList();

            if (!jobs.Any())
                return;

            var faker = new Faker();

            var metrics = jobs.Select(job => new JobMetric
            {
                JobID = job.JobID,
                Views = faker.Random.Int(10, 5000),
                ApplicationCount = faker.Random.Int(0, 300),
                LastUpdated = faker.Date.Recent(30)
            }).ToList();

            context.JobMetrics.AddRange(metrics);
            await context.SaveChangesAsync();
        }
    }
}
