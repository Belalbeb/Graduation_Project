using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class SubscriptionPlanSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.subscriptionPlans.Any())
                return;

            var plans = new List<SubscriptionPlan>
            {
                new()
                {
                    Name = SeedDependencies.FreePlanName,
                    ShortDescription = "Default plan assigned when a company registers.",
                    MonthlyPrice = 0,
                    YearlyPrice = 0,
                    MaxJobPostsPerMonth = 2,
                    FeaturedJobPostsPerMonth = 0,
                    HasAiToolsAccess = false,
                    HasCandidateSearch = false,
                    HasPrioritySupport = false,
                    IsPublished = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-6)
                },
                new()
                {
                    Name = "Professional",
                    ShortDescription = "For growing teams that need more visibility and AI tools.",
                    MonthlyPrice = 49.99m,
                    YearlyPrice = 499.99m,
                    MaxJobPostsPerMonth = 15,
                    FeaturedJobPostsPerMonth = 3,
                    HasAiToolsAccess = true,
                    HasCandidateSearch = true,
                    HasPrioritySupport = false,
                    IsPublished = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-6)
                },
                new()
                {
                    Name = "Enterprise",
                    ShortDescription = "Unlimited hiring power with priority support and advanced search.",
                    MonthlyPrice = 149.99m,
                    YearlyPrice = 1499.99m,
                    MaxJobPostsPerMonth = 50,
                    FeaturedJobPostsPerMonth = 10,
                    HasAiToolsAccess = true,
                    HasCandidateSearch = true,
                    HasPrioritySupport = true,
                    IsPublished = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-6)
                }
            };

            context.subscriptionPlans.AddRange(plans);
            await context.SaveChangesAsync();
        }
    }
}
