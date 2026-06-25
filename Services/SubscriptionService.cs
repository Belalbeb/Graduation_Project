using Graduation_Project.Dtos;
using Graduation_Project.DTOs.Subscriptions;
using Graduation_Project.Models;
using Graduation_Project.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Reflection.Metadata.Ecma335;

namespace Graduation_Project.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionService(ApplicationDbContext context, ISubscriptionRepository subscriptionRepository,ICompanyRepository companyRepository)
        {
            _context = context;
            SubscriptionRepository = subscriptionRepository;
            CompanyRepository = companyRepository;
        }

        public ISubscriptionRepository SubscriptionRepository { get; }
        public ICompanyRepository CompanyRepository { get; }

        public async Task SubscribeAsync(Guid companyId, CreateSubscriptionDto dto)
        {
            var plan = await _context.subscriptionPlans
                .FirstOrDefaultAsync(x => x.Id == dto.SubscriptionPlanId);

            if (plan == null)
                throw new Exception("Plan not found");

            var startDate = DateTime.UtcNow;

            BillingCycle billingCycle = dto.BillingCycle?.ToLower() switch
            {
                "monthly" => BillingCycle.Monthly,
                "yearly" => BillingCycle.Yearly,
                _ => throw new ArgumentException("Invalid billing cycle")
            };

            var endDate = billingCycle == BillingCycle.Monthly
                ? startDate.AddMonths(1)
                : startDate.AddYears(1);

            var amount = billingCycle == BillingCycle.Monthly
                ? plan.MonthlyPrice
                : plan.YearlyPrice;
            var subscription = new CompanySubscription
            {
                CompanyId = companyId,
                SubscriptionPlanId = plan.Id,
                BillingCycle = billingCycle,
                StartDate = startDate,
                EndDate = endDate,
                PaidAmount = amount,
                IsActive = true
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

        public async Task CreateFromStripeAsync(
     Guid companyId,
     Guid planId,
     string billingCycle,
     long amountTotal)
        {
            var plan = await _context.subscriptionPlans
                .FirstOrDefaultAsync(x => x.Id == planId);

            if (plan == null)
                throw new Exception("Plan not found");

            var activeSubscription = await SubscriptionRepository
                .GetActiveSubscription(companyId);

            // إذا كان يوجد اشتراك نشط، قم بإنهائه
            if (activeSubscription != null)
            {
                activeSubscription.IsActive = false;
                activeSubscription.EndDate = DateTime.UtcNow;

                await SubscriptionRepository.Update(activeSubscription);
            }

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
                IsActive = true
            };

            await SubscriptionRepository.AddAsync(subscription);
        }
        public async Task<CompanySubscription> GetActiveSubscriptionForCompany(Guid companyId)
        {
            var subscription = await SubscriptionRepository.GetActiveSubscription(companyId);
            return subscription;
        }
        public async Task<bool> HasCandidateSearch(Guid companyId)
        {
            var subscription = await SubscriptionRepository.GetActiveSubscription(companyId);
            if (subscription.SubscriptionPlan.HasCandidateSearch)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> HasAiToolAccess(Guid companyId)
        {
            var subscription = await SubscriptionRepository.GetActiveSubscription(companyId);
            return subscription.SubscriptionPlan.HasAiToolsAccess;
        }
        public async Task<bool> HasReachTheMaxJobPosting(Guid companyId)
        {
            var subscription = await SubscriptionRepository.GetActiveSubscription(companyId);
            int numberOfJobPostingForThisMonth =
                await CompanyRepository.CountCompanyJobsPerMonthAsync(companyId);

            return numberOfJobPostingForThisMonth >=
                   subscription.SubscriptionPlan.MaxJobPostsPerMonth;
        }
        public async Task UpdateAsync(CompanySubscription companySubscription)
        {
            await SubscriptionRepository.Update(companySubscription);
        }
    }
}