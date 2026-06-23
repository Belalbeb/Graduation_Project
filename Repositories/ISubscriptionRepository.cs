using Graduation_Project.Dtos;
using Graduation_Project.DTOs.Subscriptions;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ISubscriptionRepository
    {
        Task AddAsync(CompanySubscription subscription);
        Task<List<SubscriptionDetailsDto>> GetAllAsync();
        Task<SubscriptionDashboardDto> GetDashboardAsync();
        Task<CompanySubscription?> GetSubscriptionAsync(Guid companyId);

        Task<int> GetActiveJobsCountAsync(Guid companyId);

        Task<int> GetFeaturedJobsCountAsync(Guid companyId);

        Task<SubscriptionFullDetailsDto?> GetFullDetailsBySubscriptionIdAsync(Guid subscriptionId);
        Task<CompanySubscriptionPageDto?> GetSubscriptionPageAsync(Guid companyId);
        Task<CompanySubscription> GetActiveSubscription(Guid companyId);
        Task Update(CompanySubscription companySubscription);

    }
}
