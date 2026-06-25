using Graduation_Project.Dtos;
using Graduation_Project.DTOs.Subscriptions;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface ISubscriptionService
    {
        Task SubscribeAsync(Guid companyId, CreateSubscriptionDto dto);
       Task<List<SubscriptionDetailsDto>> GetAllAsync();
        Task<SubscriptionDashboardDto> GetDashboardAsync();
        Task<SubscriptionFullDetailsDto> GetFullDetailsBySubscriptionIdAsync(Guid subscriptionId);
        Task<CompanySubscriptionPageDto> GetSubscriptionPageAsync(Guid companyId);
        Task CreateFromStripeAsync(
       Guid companyId,
       Guid planId,
       string billingCycle,
       long amountTotal);
        Task<CompanySubscription> GetActiveSubscriptionForCompany(Guid companyId);
        Task<bool> HasCandidateSearch(Guid companyId);
        Task<bool> HasReachTheMaxJobPosting(Guid companyId);
         Task<bool> HasAiToolAccess(Guid companyId);
       Task UpdateAsync(CompanySubscription companySubscription);
    }
}
