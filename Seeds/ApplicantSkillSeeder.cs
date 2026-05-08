using Bogus;
using Graduation_Project.Models;
using System;

namespace Graduation_Project.Seeds
{
    public class ApplicantSkillSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.ApplicantSkills.Any())
                return;

            var applicants = context.Applicants.ToList();
            var skills = context.Skills.ToList();

            if (!applicants.Any() || !skills.Any())
                return;

            var levels = new[] { "Beginner", "Intermediate", "Advanced", "Expert" };

            var faker = new Faker<ApplicantSkill>()
                .RuleFor(x => x.ApplicantID,
                    f => f.PickRandom(applicants).ApplicantID)

                .RuleFor(x => x.SkillID,
                    f => f.PickRandom(skills).SkillID)

                .RuleFor(x => x.ProficiencyLevel,
                    f => f.PickRandom(levels));

            // generate more records than applicants for realism
            var data = faker.Generate(30);

            // optional: remove duplicates (important for many-to-many)
            var uniqueData = data
                .GroupBy(x => new { x.ApplicantID, x.SkillID })
                .Select(g => g.First())
                .ToList();

            context.ApplicantSkills.AddRange(uniqueData);
            await context.SaveChangesAsync();
        }
    }
}