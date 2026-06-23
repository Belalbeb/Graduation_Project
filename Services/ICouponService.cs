using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface ICouponService
    {
        Task<CouponResponseDto> CreateAsync(CreateCouponDto dto);

        Task<List<CouponResponseDto>> GetAllAsync(QueryCouponDto query);

        Task<CouponResponseDto> GetByIdAsync(Guid id);

        Task<CouponResponseDto> GetByCodeAsync(string code);

        Task<CouponResponseDto> UpdateAsync(Guid id, UpdateCouponDto dto);

        Task DeleteAsync(Guid id);

        Task<CouponValidationResult> ValidateAsync(ValidateCouponDto dto);
        Task<decimal> CalculateFinalPriceAsync(CalculatePriceDto dto);

        Task ApplyAsync(ApplyCouponDto dto);

        Task RevokeAsync(Guid couponId);

   
    }
}