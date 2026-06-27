using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class JobSkillSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.JobSkills.Any())
                return;

            var jobs = context.JobPostings.ToList();
            var skills = context.Skills.ToList();

            if (!jobs.Any() || !skills.Any())
                return;

            var faker = new Faker();
            var jobSkills = new List<JobSkill>();

            foreach (var job in jobs)
            {
                var count = faker.Random.Int(2, 5);
                var pickedSkills = faker.PickRandom(skills, count).ToList();

                foreach (var skill in pickedSkills)
                {
                    jobSkills.Add(new JobSkill
                    {
                        Name = skill.SkillName,
                        JobPostingId = job.JobID
                    });
                }
            }

            context.JobSkills.AddRange(jobSkills);
            await context.SaveChangesAsync();
        }
    }
}
