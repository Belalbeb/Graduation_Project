using Bogus;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Seeds
{
    public class JobPostingSeeder
    {
        private static readonly string[] JobTitles =
        {
            "Senior Software Engineer", "Full Stack Developer", "Backend Developer",
            "Frontend Developer", "Data Scientist", "ML Engineer", "DevOps Engineer",
            "Cloud Architect", "Mobile Developer (iOS)", "Mobile Developer (Android)",
            "UI/UX Designer", "Product Manager", "QA Engineer", "Security Engineer",
            "Site Reliability Engineer", "Database Administrator", "Solutions Architect",
            "React Developer", "Node.js Developer", "Python Developer"
        };

        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.JobPostings.Any())
                return;

            var companies = await context.Companies.ToListAsync();
            if (!companies.Any())
                return;

            var categories = new[]
            {
                "Software Development", "Data Science", "Cyber Security",
                "Web Development", "Mobile Development", "UI/UX Design",
                "DevOps", "Cloud Computing", "AI / Machine Learning",
                "QA / Testing", "IT Support", "Networking"
            };

            var jobTypes = Enum.GetValues<JobType>();
            var workApproaches = Enum.GetValues<WorkApproach>();
            var faker = new Faker();
            var jobs = new List<JobPosting>();
            var metrics = new List<JobMetric>();

            foreach (var company in companies)
            {
                var subscription = await SeedDependencies.GetActiveSubscriptionAsync(context, company.CompanyID);
                var maxJobs = subscription?.SubscriptionPlan?.MaxJobPostsPerMonth ?? 2;
                var jobCount = faker.Random.Int(1, Math.Max(1, maxJobs));

                for (int i = 0; i < jobCount; i++)
                {
                    var workApproach = faker.PickRandom(workApproaches);
                    var minExperience = faker.Random.Int(0, 4);
                    var minSalary = Math.Round(faker.Random.Decimal(3000, 8000) / 500) * 500; // round to $500
                    var postedDate = faker.Date.Past(1);
                    var title = faker.PickRandom(JobTitles);

                    var job = new JobPosting
                    {
                        Title = title,
                        Description = faker.Lorem.Paragraphs(3),
                        Responsibility = string.Join(" ", Enumerable.Range(0, 4).Select(_ => "• " + faker.Lorem.Sentence())),
                        MinSalary = minSalary,
                        MaxSalary = minSalary + Math.Round(faker.Random.Decimal(1000, 8000) / 500) * 500,
                        Location = company.Location ?? faker.Address.City(),
                        JobCategory = faker.PickRandom(categories),
                        PostedDate = postedDate,
                       
                        IsActive = faker.Random.Bool(0.85f), // 85% active
                        IsFeatured = subscription?.SubscriptionPlan?.FeaturedJobPostsPerMonth > 0
                            && faker.Random.Bool(0.3f),
                        JobTypes = new List<JobType> { faker.PickRandom(jobTypes) },
                        WorkApproaches = new List<WorkApproach> { workApproach },
                        IsRemote = workApproach == WorkApproach.Remote,
                        MinExperience = minExperience,
                        MaxExperience = minExperience + faker.Random.Int(2, 7),
                        Status = JobStatus.Approved,
                        CompanyID = company.CompanyID
                    };

                    jobs.Add(job);

                    metrics.Add(new JobMetric
                    {
                        JobID = job.JobID,
                        Views = faker.Random.Int(50, 5000),
                        ApplicationCount = 0,
                        LastUpdated = postedDate.AddDays(faker.Random.Int(1, 14))
                    });
                }
            }

            context.JobPostings.AddRange(jobs);
            context.JobMetrics.AddRange(metrics);
            await context.SaveChangesAsync();
        }
    }
}
