using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class ResumeSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Resumes.Any())
                return;

            var applicants = context.Applicants.ToList();

            if (!applicants.Any())
                return;

            var faker = new Faker();
            var resumes = new List<Resume>();

            foreach (var applicant in applicants)
            {
                resumes.Add(new Resume
                {
                    FileName = $"CV_{applicant.FirstName}_{applicant.LastName}.pdf",
                    FilePath = $"/uploads/resumes/{Guid.NewGuid()}.pdf",
                    UploadDate = faker.Date.Past(1),
                    IsActive = true,
                    ApplicantID = applicant.ApplicantID
                });
            }

            context.Resumes.AddRange(resumes);
            await context.SaveChangesAsync();
        }
    }
}
