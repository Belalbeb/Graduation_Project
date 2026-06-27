using Bogus;
using Graduation_Project.Models;

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
            var faker = new Faker();
            var applicantSkills = new List<ApplicantSkill>();

            foreach (var applicant in applicants)
            {
                var skillCount = faker.Random.Int(3, Math.Min(6, skills.Count));
                var pickedSkills = faker.PickRandom(skills, skillCount).DistinctBy(s => s.SkillID);

                foreach (var skill in pickedSkills)
                {
                    applicantSkills.Add(new ApplicantSkill
                    {
                        ApplicantID = applicant.ApplicantID,
                        SkillID = skill.SkillID,
                        ProficiencyLevel = faker.PickRandom(levels)
                    });
                }
            }

            context.ApplicantSkills.AddRange(applicantSkills);
            await context.SaveChangesAsync();
        }
    }
}
