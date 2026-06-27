using Graduation_Project.Models;
using Graduation_Project.Seeds.Identity;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Seeds
{
    public class SeederRunner
    {
        public static async Task Run(IServiceProvider sp)
        {
            var context = sp.GetRequiredService<ApplicationDbContext>();
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Identity
            await RoleSeeder.SeedRoles(roleManager);
            await UserSeeder.SeedUsers(userManager);
            await UserRoleSeeder.Seed(userManager);

            // 2. Core reference data
            await SkillSeeder.Seed(context);
            await WebsiteSettingsSeeder.Seed(context);
            await SubscriptionPlanSeeder.Seed(context);

            // 3. Profiles (company gets Free plan on creation — same as register/company)
            await CompanySeeder.Seed(context, userManager);
            await ApplicantSeeder.Seed(context, userManager);

            // 4. Optional paid upgrades before jobs (respects plan limits)
            await CompanySubscriptionSeeder.Seed(context);

            // 5. Applicant-owned data
            await ExperienceSeeder.Seed(context);
            await ProjectSeeder.Seed(context);
            await ResumeSeeder.Seed(context);
            await ApplicantSkillSeeder.Seed(context);

            // 6. Company-owned jobs (count limited by active subscription)
            await JobPostingSeeder.Seed(context);
            await JobSkillSeeder.Seed(context);

            // 7. Applicant interactions with jobs
            await SavedJobsSeeder.Seed(context);
            await ProfileViewSeeder.Seed(context);
            await ApplicationSeeder.Seed(context);
            await InterviewSeeder.Seed(context);

            // 8. Billing & verification
            await CouponSeeder.Seed(context);
            //await CouponSubscriptionPlanSeeder.Seed(context);
            await CompanyVerificationRequestSeeder.Seed(context);
        }
    }
}
