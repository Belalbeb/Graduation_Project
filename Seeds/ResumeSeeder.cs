using Bogus;
using Graduation_Project.Models;
using System;

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

            var faker = new Faker<Resume>()
                .RuleFor(r => r.FileName,
                    f => $"CV_{f.Name.FirstName()}_{f.Name.LastName()}.pdf")

                .RuleFor(r => r.FilePath,
                    f => $"/uploads/resumes/{Guid.NewGuid()}.pdf")

                .RuleFor(r => r.UploadDate,
                    f => f.Date.Past(1))

                .RuleFor(r => r.IsActive,
                    f => true)

                .RuleFor(r => r.ApplicantID,
                    f => f.PickRandom(applicants).ApplicantID);

            var resumes = faker.Generate(10);

            context.Resumes.AddRange(resumes);
            await context.SaveChangesAsync();
        }
    }
}