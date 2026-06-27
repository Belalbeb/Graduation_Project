using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class ProfileViewSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.ProfileViews.Any())
                return;

            var applicants = context.Applicants.ToList();

            if (!applicants.Any())
                return;

            var faker = new Faker();
            var profileViews = new List<ProfileView>();

            foreach (var applicant in applicants)
            {
                // Weight view counts realistically: most get few, some get many
                var viewCount = faker.Random.Bool(0.2f)
                    ? faker.Random.Int(30, 100)   // 20% popular profiles
                    : faker.Random.Int(3, 30);    // 80% normal profiles

                for (int i = 0; i < viewCount; i++)
                {
                    profileViews.Add(new ProfileView
                    {
                        ApplicantId = applicant.ApplicantID
                    });
                }
            }

            context.ProfileViews.AddRange(profileViews);
            await context.SaveChangesAsync();
        }
    }
}
