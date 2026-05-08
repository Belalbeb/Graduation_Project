
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

            var faker = new Faker<Experience>()
                .RuleFor(e => e.CompanyName,
                    f => f.Company.CompanyName())

                .RuleFor(e => e.Location,
                    f => f.Address.City())

                .RuleFor(e => e.JobTitle,
                    f => f.Name.JobTitle())

                .RuleFor(e => e.Description,
                    f => f.Lorem.Paragraph())

                .RuleFor(e => e.JobType,
                    f => f.PickRandom<JobType>())

      
                .RuleFor(e => e.StartDate,
                    f => f.Date.Past(5))

                .RuleFor(e => e.EndDate,
                    (f, e) => e.StartDate.AddMonths(f.Random.Int(3, 36)))

                .RuleFor(e => e.ApplicantID,
                    f => f.PickRandom(applicants).ApplicantID);

            var experiences = faker.Generate(20);

            context.Experiences.AddRange(experiences);
            await context.SaveChangesAsync();
        }
    }
}