using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Seeds
{
    /// <summary>
    /// Shared helpers that mirror real registration / business flows.
    /// </summary>
    public static class SeedDependencies
    {
        public const string FreePlanName = "Free";

        public static async Task<SubscriptionPlan> GetFreePlanAsync(ApplicationDbContext context)
        {
            return await context.subscriptionPlans
                .FirstAsync(x => x.Name == FreePlanName);
        }

        /// <summary>
        /// Same flow as AuthController company registration: assign Free plan on signup.
        /// </summary>
        public static CompanySubscription CreateFreeSubscription(Guid companyId, Guid freePlanId, DateTime? startDate = null)
        {
            var start = startDate ?? DateTime.UtcNow;

            return new CompanySubscription
            {
                CompanyId = companyId,
                SubscriptionPlanId = freePlanId,
                BillingCycle = BillingCycle.Monthly,
                StartDate = start,
                EndDate =null,
                PaidAmount = 0,
                IsActive = true,
                CreatedAt = start
            };
        }

        public static async Task<CompanySubscription?> GetActiveSubscriptionAsync(
            ApplicationDbContext context,
            Guid companyId)
        {
            return await context.companySubscriptions
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.CompanyId == companyId && s.IsActive);
        }
    }
}
