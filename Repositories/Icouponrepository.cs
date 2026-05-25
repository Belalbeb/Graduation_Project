using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface Icouponrepository
    {
        Task<Coupon> CreateAsync(Coupon coupon);

        Task<Coupon?> GetByIdAsync(Guid id);

        Task<Coupon?> GetByCodeAsync(string code);

        Task<PaginatedResult<Coupon>> GetAllAsync(QueryCouponDto query);

        Task<Coupon?> GetValidCouponForPlanAsync(string code, Coupon.ApplicablePlan plan);

        Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null);

        Task<Coupon> UpdateAsync(Coupon coupon);

        Task IncrementUsageAsync(Guid id);

        Task DecrementUsageAsync(Guid id);

        Task SoftDeleteAsync(Guid id);

        Task RestoreAsync(Guid id);

        Task HardDeleteAsync(Guid id);
    }
}