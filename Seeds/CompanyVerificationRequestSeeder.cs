using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class CompanyVerificationRequestSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.companyVerificationRequests.Any())
                return;

            var companies = context.Companies.ToList();

            if (!companies.Any())
                return;

            var faker = new Faker();
            var requests = new List<CompanyVerificationRequest>();

            foreach (var company in companies)
            {
                if (!faker.Random.Bool(0.5f))
                    continue;

                var status = faker.PickRandom<VerificationStatus>();
                var submittedAt = faker.Date.Past(1);

                requests.Add(new CompanyVerificationRequest
                {
                    CompanyId = company.CompanyID,
                    Status = status,
                    SubmittedAt = submittedAt,
                    AdminNotes = status is VerificationStatus.Rejected or VerificationStatus.NeedsMoreInformation
                        ? faker.Lorem.Sentence()
                        : status == VerificationStatus.Approved
                            ? "Documents verified successfully."
                            : null,
                    Documents = new List<VerificationDocument>
                    {
                        new()
                        {
                            FileName = "business_license.pdf",
                            FileUrl = $"https://res.cloudinary.com/demo/raw/upload/v1/docs/{Guid.NewGuid()}.pdf",
                            FileSize = faker.Random.Float(100, 5000),
                            UploadedAt = submittedAt
                        },
                        new()
                        {
                            FileName = "tax_registration.pdf",
                            FileUrl = $"https://res.cloudinary.com/demo/raw/upload/v1/docs/{Guid.NewGuid()}.pdf",
                            FileSize = faker.Random.Float(100, 3000),
                            UploadedAt = submittedAt.AddHours(faker.Random.Int(1, 48))
                        }
                    }
                });

                if (status == VerificationStatus.Approved)
                    company.Status = CompanyStatus.Verified;
            }

            context.companyVerificationRequests.AddRange(requests);
            await context.SaveChangesAsync();
        }
    }
}
