using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class SavedJobsSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.SavedJobs.Any())
                return;

            var applicants = context.Applicants.ToList();
            var jobs = context.JobPostings.ToList();

            if (!applicants.Any() || !jobs.Any())
                return;

            var faker = new Faker();
            var savedJobs = new List<SavedJobs>();

            foreach (var applicant in applicants)
            {
                var saveCount = faker.Random.Int(1, Math.Min(5, jobs.Count));
                var pickedJobs = faker.PickRandom(jobs, saveCount).DistinctBy(j => j.JobID);

                foreach (var job in pickedJobs)
                {
                    savedJobs.Add(new SavedJobs
                    {
                        ApplicantId = applicant.ApplicantID,
                        JobPostingId = job.JobID,
                        SavedAt = faker.Date.Past(1)
                    });
                }
            }

            context.SavedJobs.AddRange(savedJobs);
            await context.SaveChangesAsync();
        }
    }
}
