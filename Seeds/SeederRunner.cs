using Microsoft.AspNetCore.Identity;
using Graduation_Project.Models;
using Graduation_Project.Seeds.Identity;

namespace Graduation_Project.Seeds
{
    public class SeederRunner
    {
        public static async Task Run(IServiceProvider sp)
        {
            var context = sp.GetRequiredService<ApplicationDbContext>();
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

            // =======================
            // 1. IDENTITY
            // =======================
            await RoleSeeder.SeedRoles(roleManager);

            //await UserSeeder.SeedUsers(userManager);
            //await UserRoleSeeder.Seed(userManager);

            //// =======================
            //// 2. CORE DATA
            //// =======================
            //await SkillSeeder.Seed(context);

            //// =======================
            //// 3. USERS PROFILES
            //// =======================
            //await CompanySeeder.Seed(context,userManager);
            //await ApplicantSeeder.Seed(context,userManager);

            //// =======================
            //// 4. APPLICANT DETAILS
            //// =======================
            //await ExperienceSeeder.Seed(context);
            //await ProjectSeeder.Seed(context);
            //await ResumeSeeder.Seed(context);

            //// =======================
            //// 5. JOB SYSTEM
            //// =======================
            //await JobPostingSeeder.Seed(context);
            //await JobMetricSeeder.Seed(context);

            //// =======================
            //// 6. RELATIONS
            //// =======================
            //await ApplicantSkillSeeder.Seed(context);
            //await SavedJobsSeeder.Seed(context);

            //// =======================
            //// 7. TRANSACTIONS
            //// =======================
            //await ApplicationSeeder.Seed(context);
            //await InterviewSeeder.Seed(context);
        }
    }
}