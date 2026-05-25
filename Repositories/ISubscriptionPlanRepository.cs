using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ISubscriptionPlanRepository
    {
         Task<List<SubscriptionPlanResponseDto>> GetAllAsync();
        Task AddAsync(SubscriptionPlan subscription);
        Task<SubscriptionPlan> GetById(Guid Id);
      Task update(SubscriptionPlan plan);
        Task Delete(SubscriptionPlan plan);
    }
}
