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

            var metrics = jobs.Select(job => new JobMetric
            {
                JobID = job.JobID,
                Views = Random.Shared.Next(0, 5000),
                ApplicationCount = Random.Shared.Next(0, 300),
                LastUpdated = DateTime.UtcNow
            }).ToList();

            context.JobMetrics.AddRange(metrics);

            await context.SaveChangesAsync();
        }
    }
}