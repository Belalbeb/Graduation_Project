using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface ISubscriptionPlanService
    {
       Task<List<SubscriptionPlanResponseDto>> GetAllAsync();
        Task<SubscriptionPlanResponseDto> GetByIdAsync(Guid id);
        Task AddAsync(AddSubscriptionPlanDto dto);
        Task<bool> DeleteAsync(Guid Id);
        Task<bool> UpdateAsync(Guid id, UpdateSubscriptionPlanDto dto);
    }
}
