using Bogus;
using Microsoft.AspNetCore.Identity;
using Graduation_Project.Models;

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

            var faker = new Faker<Applicant>()
                .RuleFor(a => a.FirstName, f => f.Name.FirstName())
                .RuleFor(a => a.LastName, f => f.Name.LastName())
                .RuleFor(a => a.JobTitle, f => f.Name.JobTitle())
                .RuleFor(a => a.AboutMe, f => f.Lorem.Paragraph())
                .RuleFor(a => a.Location, f => f.Address.City())

                .RuleFor(a => a.Email,
                    (f, a) => f.Internet.Email(a.FirstName, a.LastName))

                .RuleFor(a => a.PhoneNumber,
                    f => f.Phone.PhoneNumber())

                .RuleFor(a => a.ProfilePicURL,
                    f => $"https://i.pravatar.cc/150?u={Guid.NewGuid()}")

                .RuleFor(a => a.CoverPhotoUrl,
                    f => $"https://picsum.photos/seed/{Guid.NewGuid()}/800/300")

                .RuleFor(a => a.Linkedin,
                    (f, a) => $"https://linkedin.com/in/{a.FirstName.ToLower()}{a.LastName.ToLower()}")

                .RuleFor(a => a.Github,
                    f => $"https://github.com/{f.Internet.UserName()}")

                .RuleFor(a => a.Portfolio,
                    f => f.Internet.Url())

                .RuleFor(a => a.Facebook,
                    f => f.Internet.Url())

                .RuleFor(a => a.UserId,
                    f => f.PickRandom(applicantUsers).Id);

            var data = faker.Generate(applicantUsers.Count);

            context.Applicants.AddRange(data);
            await context.SaveChangesAsync();
        }
    }
}