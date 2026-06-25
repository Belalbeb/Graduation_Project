using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Graduation_Project.BackgroundJobs
{


    public class SubscriptionExpirationJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SubscriptionExpirationJob(
            IServiceScopeFactory scopeFactory)
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
                .Where(s =>
                        s.IsActive &&
                        s.EndDate <= DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                foreach (var subscription in expiredSubscriptions)
                {
                    subscription.IsActive = false;
                       
                }

                await context.SaveChangesAsync(stoppingToken);

                // تتكرر كل ساعة
                await Task.Delay(
                    TimeSpan.FromDays(1),
                    stoppingToken);
            }
        }
    }
}
