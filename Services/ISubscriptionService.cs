using Graduation_Project.Dtos;
using Graduation_Project.DTOs.Subscriptions;

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
    }
}
