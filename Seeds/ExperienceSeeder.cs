using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class ExperienceSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Experiences.Any())
                return;

            var applicants = context.Applicants.ToList();

            if (!applicants.Any())
                return;

            var faker = new Faker();
            var experiences = new List<Experience>();

            foreach (var applicant in applicants)
            {
                var count = faker.Random.Int(2, 5);
                var cursor = DateTime.UtcNow;

                for (int i = 0; i < count; i++)
                {
                    // Build chronologically backwards so no overlapping dates
                    var durationMonths = faker.Random.Int(6, 36);
                    var endDate = cursor.AddDays(-faker.Random.Int(0, 90)); // gap between jobs
                    var startDate = endDate.AddMonths(-durationMonths);
                    cursor = startDate;

                    var isCurrent = i == 0 && faker.Random.Bool(0.4f);

                    experiences.Add(new Experience
                    {
                        CompanyName = faker.Company.CompanyName(),
                        Location = faker.Address.City(),
                        JobTitle = faker.Name.JobTitle(),
                        Description = faker.Lorem.Paragraph(),
                        JobType = faker.PickRandom<JobType>(),
                        StartDate = startDate,
                        EndDate = isCurrent ? null : endDate,
                        ApplicantID = applicant.ApplicantID
                    });
                }
            }

            context.Experiences.AddRange(experiences);
            await context.SaveChangesAsync();
        }
    }
}
