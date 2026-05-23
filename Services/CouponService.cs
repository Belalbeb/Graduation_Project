using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class CouponService : ICouponService
    {
        private readonly Icouponrepository _repository;

        public CouponService(Icouponrepository repository)
        {
            _repository = repository;
        }

        public async Task<CouponResponseDto> CreateAsync(CreateCouponDto dto)
        {
            var code = dto.Code.ToUpper().Trim();

            if (await _repository.ExistsByCodeAsync(code))
                throw new Exception("Coupon already exists");

            var coupon = new Coupon
            {
                Code = code,
                Percentage = dto.Percentage,
                TotalUsageLimit = dto.TotalUsageLimit,
                IsActive = dto.IsActive,
                ApplicablePlans = dto.ApplicablePlans,
                ExpiresAt = dto.ExpiresAt
            };

            var result = await _repository.CreateAsync(coupon);
            return Map(result);
        }

        public async Task<PaginatedResult<CouponResponseDto>> GetAllAsync(QueryCouponDto query)
        {
            var result = await _repository.GetAllAsync(query);

            return new PaginatedResult<CouponResponseDto>
            {
                Data = result.Data.Select(Map).ToList(),
                Total = result.Total,
                Page = result.Page,
                Limit = result.Limit,
                TotalPages = result.TotalPages
            };
        }

        public async Task<CouponResponseDto> GetByIdAsync(Guid id)
        {
            var coupon = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Not found");

            return Map(coupon);
        }

        public async Task<CouponResponseDto> UpdateAsync(Guid id, UpdateCouponDto dto)
        {
            var coupon = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Not found");

            if (dto.Code != null)
                coupon.Code = dto.Code.ToUpper().Trim();

            if (dto.Percentage.HasValue)
                coupon.Percentage = dto.Percentage.Value;

            if (dto.TotalUsageLimit.HasValue)
                coupon.TotalUsageLimit = dto.TotalUsageLimit.Value;

            if (dto.IsActive.HasValue)
                coupon.IsActive = dto.IsActive.Value;

            if (dto.ApplicablePlans != null)
                coupon.ApplicablePlans = dto.ApplicablePlans;

            if (dto.ExpiresAt.HasValue)
                coupon.ExpiresAt = dto.ExpiresAt.Value;

            var updated = await _repository.UpdateAsync(coupon);
            return Map(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.SoftDeleteAsync(id);
        }

        public async Task<CouponValidationResult> ValidateAsync(ValidateCouponDto dto)
        {
            var coupon = await _repository.GetValidCouponForPlanAsync(dto.Code, dto.Plan)
                ?? throw new Exception("Invalid coupon");

            return new CouponValidationResult
            {
                Valid = true,
                Coupon = Map(coupon),
                DiscountAmount = coupon.Percentage
            };
        }

        public async Task ApplyAsync(ApplyCouponDto dto)
        {
            var result = await ValidateAsync(dto);
            await _repository.IncrementUsageAsync(result.Coupon.Id);
        }

        private static CouponResponseDto Map(Coupon c)
        {
            return new CouponResponseDto
            {
                Id = c.Id,
                Code = c.Code,
                Percentage = c.Percentage,
                TotalUsageLimit = c.TotalUsageLimit,
                UsedCount = c.UsedCount,
                RemainingUses = c.RemainingUses,
                IsActive = c.IsActive,
                Status = c.Status,
                ApplicablePlans = c.ApplicablePlans,
                ExpiresAt = c.ExpiresAt,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            };
        }

        public Task<CouponResponseDto> GetByCodeAsync(string code)
        {
            throw new NotImplementedException();
        }

        public Task RevokeAsync(Guid couponId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneratedCodeDto> GenerateCodeAsync(string? prefix = null)
        {
            throw new NotImplementedException();
        }
    }
}