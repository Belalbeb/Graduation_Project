using Bogus;
using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Seeds
{
    public class ApplicantSeeder
    {
        public static async Task Seed(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            if (context.Applicants.Any())
                return;

            var applicantUsers = await userManager.GetUsersInRoleAsync(Roles.Applicant);

            if (!applicantUsers.Any())
                return;

            var industries = new[]
            {
                "Software Development", "Data Science", "UI/UX Design",
                "DevOps", "Mobile Development", "Cyber Security"
            };

            var faker = new Faker();

            var applicants = applicantUsers.Select(user =>
            {
                var firstName = faker.Name.FirstName();
                var lastName = faker.Name.LastName();
                var username = $"{firstName.ToLower()}.{lastName.ToLower()}{faker.Random.Int(1, 99)}";

                return new Applicant
                {
                    FirstName = firstName,
                    LastName = lastName,
                    JobTitle = faker.Name.JobTitle(),
                    Industry = faker.PickRandom(industries),
                    Address = faker.Address.StreetAddress(),
                    AboutMe = faker.Lorem.Paragraph(),
                    Location = faker.Address.City(),
                    PhoneNumber = faker.Phone.PhoneNumber("+1-###-###-####"),
                    ProfilePicURL = $"https://i.pravatar.cc/150?u={user.Id}",
                    CoverPhotoUrl = $"https://picsum.photos/seed/{user.Id}/800/300",
                    Linkedin = $"https://linkedin.com/in/{firstName.ToLower()}-{lastName.ToLower()}-{faker.Random.AlphaNumeric(6)}",
                    Github = $"https://github.com/{username}",
                    Facebook = $"https://facebook.com/{username}",
                    Behance = $"https://behance.net/{username}",
                    Dribble = $"https://dribbble.com/{username}",
                    Portfolio = $"https://{username}.dev",
                    UserId = user.Id
                };
            }).ToList();

            context.Applicants.AddRange(applicants);
            await context.SaveChangesAsync();
        }
    }
}
