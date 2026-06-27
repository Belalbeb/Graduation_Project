using Bogus;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Seeds
{
    public class CouponSubscriptionPlanSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            var coupons = context.Coupons
                .Include(c => c.CouponSubscriptionPlans)
                .ToList();
            var plans = context.subscriptionPlans.ToList();

            if (!coupons.Any() || !plans.Any())
                return;

            if (coupons.Any(c => c.CouponSubscriptionPlans.Any()))
                return;

            var faker = new Faker();
            var links = new List<CouponSubscriptionPlan>();

            foreach (var coupon in coupons)
            {
                var planCount = faker.Random.Int(1, Math.Min(2, plans.Count));
                var selectedPlans = faker.PickRandom(plans, planCount).ToList();

                foreach (var plan in selectedPlans)
                {
                    links.Add(new CouponSubscriptionPlan
                    {
                        CouponId = coupon.Id,
                        SubscriptionPlanId = plan.Id
                    });
                }
            }

            context.Set<CouponSubscriptionPlan>().AddRange(links);
            await context.SaveChangesAsync();
        }
    }
}
