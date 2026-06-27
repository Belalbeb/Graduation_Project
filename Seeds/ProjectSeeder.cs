using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class ProjectSeeder
    {
        private static readonly string[] ProjectAdjectives =
            { "Smart", "Real-Time", "AI-Powered", "Automated", "Scalable", "Cloud-Based", "Open Source" };

        private static readonly string[] ProjectNouns =
            { "Dashboard", "API", "Platform", "App", "System", "Tool", "Bot", "Tracker", "Manager", "Analyzer" };

        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Projects.Any())
                return;

            var applicants = context.Applicants.ToList();

            if (!applicants.Any())
                return;

            var faker = new Faker();
            var projects = new List<Project>();

            foreach (var applicant in applicants)
            {
                var count = faker.Random.Int(1, 4);

                for (int i = 0; i < count; i++)
                {
                    var adjective = faker.PickRandom(ProjectAdjectives);
                    var noun = faker.PickRandom(ProjectNouns);
                    var title = $"{adjective} {faker.Commerce.Department()} {noun}";
                    var repoName = title.ToLower().Replace(" ", "-");
                    var createdAt = faker.Date.Past(3);

                    projects.Add(new Project
                    {
                        Title = title,
                        Description = faker.Lorem.Paragraph(),
                        ProjectUrl = $"https://{repoName}.vercel.app",
                        GithubRepoUrl = $"https://github.com/{faker.Internet.UserName()}/{repoName}",
                        ImageUrl = $"https://picsum.photos/seed/{applicant.ApplicantID}-{i}/600/400",
                        CreatedAt = createdAt,
                        UpdatedAt = faker.Random.Bool(0.6f)
                            ? createdAt.AddMonths(faker.Random.Int(1, 18))
                            : null,
                        ApplicantID = applicant.ApplicantID
                    });
                }
            }

            context.Projects.AddRange(projects);
            await context.SaveChangesAsync();
        }
    }
}
