using Graduation_Project.Dtos;
using Graduation_Project.DTOs.Subscriptions;
using Graduation_Project.Models;
using Graduation_Project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionService(ApplicationDbContext context, ISubscriptionRepository subscriptionRepository)
        {
            _context = context;
            SubscriptionRepository = subscriptionRepository;
        }

        public ISubscriptionRepository SubscriptionRepository { get; }

        public async Task SubscribeAsync(Guid companyId, CreateSubscriptionDto dto)
        {
            var plan = await _context.subscriptionPlans
                .FirstOrDefaultAsync(x => x.Id == dto.SubscriptionPlanId);

            if (plan == null)
                throw new Exception("Plan not found");

            var startDate = DateTime.UtcNow;

            var endDate = dto.BillingCycle == BillingCycle.Monthly
                ? startDate.AddMonths(1)
                : startDate.AddYears(1);

            var amount = dto.BillingCycle == BillingCycle.Monthly
                ? plan.MonthlyPrice
                : plan.YearlyPrice;

            var subscription = new CompanySubscription
            {
                CompanyId = companyId,
                SubscriptionPlanId = plan.Id,
                BillingCycle = dto.BillingCycle,
                StartDate = startDate,
                EndDate = endDate,
                PaidAmount = amount,
                IsCancelled = false
            };

            await SubscriptionRepository.AddAsync(subscription);
        }
        public async Task<List<SubscriptionDetailsDto>> GetAllAsync()
        {
            return await SubscriptionRepository.GetAllAsync();

        }
        public async Task<SubscriptionDashboardDto> GetDashboardAsync()
        {
            return await SubscriptionRepository.GetDashboardAsync();
        }



        public async Task<SubscriptionFullDetailsDto> GetFullDetailsBySubscriptionIdAsync(Guid subscriptionId)
        {
            var result = await SubscriptionRepository.GetFullDetailsBySubscriptionIdAsync(subscriptionId);

            if (result is null)
                throw new KeyNotFoundException(
                    $"Subscription '{subscriptionId}' not found.");

            return result;
        }
        public async Task<CompanySubscriptionPageDto> GetSubscriptionPageAsync(Guid companyId)
        {
            var result = await SubscriptionRepository.GetSubscriptionPageAsync(companyId);

            if (result is null)
                throw new KeyNotFoundException(
                    $"No active subscription found for company '{companyId}'.");

            return result;
        }

        public async Task CreateFromStripeAsync(Guid companyId,Guid planId,string billingCycle,long amountTotal)
        {
            var plan = await _context.subscriptionPlans
    .FirstOrDefaultAsync(x => x.Id == planId);

            if (plan == null)
                throw new Exception("Plan not found");
            var startDate = DateTime.UtcNow;

            var endDate = billingCycle == "Yearly"
                ? startDate.AddYears(1)
                : startDate.AddMonths(1);
            var subscription = new CompanySubscription
            {
                CompanyId = companyId,
                SubscriptionPlanId = planId,
                BillingCycle = billingCycle == "Yearly"
        ? BillingCycle.Yearly
        : BillingCycle.Monthly,

                StartDate = startDate,
                EndDate = endDate,
                PaidAmount = amountTotal / 100m,
                IsCancelled = false
            };
            await SubscriptionRepository.AddAsync(subscription);
        }
        
        }
}