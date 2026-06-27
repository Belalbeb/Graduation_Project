using Bogus;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Seeds
{
    /// <summary>
    /// Simulates paid upgrades for a subset of companies (same flow as Stripe subscription).
    /// All companies already have Free plan from CompanySeeder.
    /// </summary>
    public class CompanySubscriptionSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            var paidPlans = await context.subscriptionPlans
                .Where(p => p.Name != SeedDependencies.FreePlanName)
                .ToListAsync();

            if (!paidPlans.Any())
                return;

            var companies = await context.Companies.ToListAsync();
            if (!companies.Any())
                return;

            var faker = new Faker();
            var upgradedCompanyIds = companies
                .OrderBy(_ => faker.Random.Int())
                .Take(Math.Min(2, companies.Count))
                .Select(c => c.CompanyID)
                .ToHashSet();

            foreach (var companyId in upgradedCompanyIds)
            {
                var activeSubscription = await SeedDependencies.GetActiveSubscriptionAsync(context, companyId);
                if (activeSubscription == null || activeSubscription.SubscriptionPlan?.Name != SeedDependencies.FreePlanName)
                    continue;

                var plan = faker.PickRandom(paidPlans);
                var billingCycle = faker.PickRandom<BillingCycle>();
                var startDate = DateTime.UtcNow.AddDays(-faker.Random.Int(1, 20));

                activeSubscription.IsActive = false;
                activeSubscription.EndDate = startDate;

                var paidAmount = billingCycle == BillingCycle.Monthly
                    ? plan.MonthlyPrice
                    : plan.YearlyPrice;

                var endDate = billingCycle == BillingCycle.Monthly
                    ? startDate.AddMonths(1)
                    : startDate.AddYears(1);

                context.companySubscriptions.Add(new CompanySubscription
                {
                    CompanyId = companyId,
                    SubscriptionPlanId = plan.Id,
                    BillingCycle = billingCycle,
                    StartDate = startDate,
                    EndDate = endDate,
                    PaidAmount = paidAmount,
                    IsActive = true,
                    CreatedAt = startDate
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
