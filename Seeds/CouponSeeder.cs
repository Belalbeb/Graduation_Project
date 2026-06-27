using Bogus;
using Graduation_Project.Models;

namespace Graduation_Project.Seeds
{
    public class CouponSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Coupons.Any())
                return;

            var faker = new Faker();
            var now = DateTime.UtcNow;

            var coupons = new List<Coupon>
            {
                new()
                {
                    Code = "WELCOME20",
                    Percentage = 20,
                    TotalUsageLimit = 100,
                    UsedCount = faker.Random.Int(5, 40),
                    IsActive = true,
                    ExpiresAt = now.AddMonths(3),
                    CreatedAt = now.AddMonths(-2),
                    UpdatedAt = now.AddDays(-5)
                },
                new()
                {
                    Code = "SUMMER50",
                    Percentage = 50,
                    TotalUsageLimit = 50,
                    UsedCount = faker.Random.Int(10, 30),
                    IsActive = true,
                    ExpiresAt = now.AddMonths(1),
                    CreatedAt = now.AddMonths(-1),
                    UpdatedAt = now.AddDays(-2)
                },
                new()
                {
                    Code = "EXPIRED10",
                    Percentage = 10,
                    TotalUsageLimit = 200,
                    UsedCount = 150,
                    IsActive = true,
                    ExpiresAt = now.AddMonths(-2),
                    CreatedAt = now.AddMonths(-6),
                    UpdatedAt = now.AddMonths(-2)
                },
                new()
                {
                    Code = "INACTIVE25",
                    Percentage = 25,
                    TotalUsageLimit = 75,
                    UsedCount = 0,
                    IsActive = false,
                    ExpiresAt = now.AddMonths(6),
                    CreatedAt = now.AddMonths(-1),
                    UpdatedAt = now.AddDays(-10)
                },
                new()
                {
                    Code = "ENTERPRISE30",
                    Percentage = 30,
                    TotalUsageLimit = 25,
                    UsedCount = faker.Random.Int(0, 10),
                    IsActive = true,
                    ExpiresAt = null,
                    CreatedAt = now.AddDays(-15),
                    UpdatedAt = now.AddDays(-1)
                }
            };

            context.Coupons.AddRange(coupons);
            await context.SaveChangesAsync();
        }
    }
}
