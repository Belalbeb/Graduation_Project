using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.BackgroundJobs
{
    public class SubscriptionExpirationJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SubscriptionExpirationJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                var expiredSubscriptions = await context.companySubscriptions
                    .Include(x => x.SubscriptionPlan)
                    .Where(x =>
                        x.IsActive &&
                        x.SubscriptionPlan.Name != "Free" &&
                        x.EndDate <= DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                foreach (var subscription in expiredSubscriptions)
                {
                    // Disable expired paid subscription
                    subscription.IsActive = false;

                    // Reactivate existing free subscription
                    var freeSubscription = await context.companySubscriptions
                        .Include(x => x.SubscriptionPlan)
                        .FirstOrDefaultAsync(x =>
                            x.CompanyId == subscription.CompanyId &&
                            x.SubscriptionPlan.Name == "Free",
                            stoppingToken);

                    if (freeSubscription != null)
                    {
                        freeSubscription.IsActive = true;
                    }
                }

                await context.SaveChangesAsync(stoppingToken);

                await Task.Delay(
                    TimeSpan.FromDays(1),
                    stoppingToken);
            }
        }
    }
}