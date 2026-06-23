using Graduation_Project.Dtos;
using Graduation_Project.Exceptions;
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
                throw new BadRequestException("Coupon already exists");

            var coupon = new Coupon
            {
                Code = code,
                Percentage = dto.Percentage,
                TotalUsageLimit = dto.TotalUsageLimit,
                IsActive = dto.IsActive,
              
                ExpiresAt = dto.ExpiresAt
            };

            // Map Many-to-Many
            if ( dto.SubscriptionPlanIds != null)
            {
                coupon.CouponSubscriptionPlans = dto.SubscriptionPlanIds
                    .Select(id => new CouponSubscriptionPlan
                    {
                        SubscriptionPlanId = id
                    })
                    .ToList();
            }

            var result = await _repository.CreateAsync(coupon);
            return Map(result);
        }

     
        public async Task<List<CouponResponseDto>> GetAllAsync(QueryCouponDto query)
        {
            var result = await _repository.GetAllAsync(query);

            return result.Select(Map).ToList();
        }

        
        public async Task<CouponResponseDto> GetByIdAsync(Guid id)
        {
            var coupon = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Not found");

            return Map(coupon);
        }

        public async Task<CouponResponseDto> GetByCodeAsync(string code)
        {
            var coupon = await _repository.GetByCodeAsync(code)
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


            // reset & rebuild plans
            if (dto.SubscriptionPlanIds != null)
            {
                coupon.CouponSubscriptionPlans.Clear();

                coupon.CouponSubscriptionPlans = dto.SubscriptionPlanIds
                    .Select(x => new CouponSubscriptionPlan
                    {
                        SubscriptionPlanId = x
                    }).ToList();
            }

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
            var coupon = await _repository
                .GetValidCouponForPlanAsync(dto.Code, dto.SubscriptionPlanId);


                if (coupon == null) return new CouponValidationResult { IsValid = false, DiscountAmount = 0 };

            return new CouponValidationResult
            {
                IsValid = true,
               
                
                DiscountAmount = coupon.Percentage
            };
        }


        public async Task ApplyAsync(ApplyCouponDto dto)
        {
            await ValidateAsync(dto);

            var coupon = await _repository.GetByCodeAsync(dto.Code);

            await _repository.IncrementUsageAsync(coupon!.Id);
        }


        public async Task RevokeAsync(Guid couponId)
        {
            var coupon = await _repository.GetByIdAsync(couponId)
                ?? throw new NotFoundException("Not found");

            coupon.IsActive = false;
            await _repository.UpdateAsync(coupon);
        }
        public async Task<decimal> CalculateFinalPriceAsync(CalculatePriceDto dto)
        {
            var coupon = await _repository
                .GetValidCouponForPlanAsync(dto.CouponCode, dto.SubscriptionPlanId)
                ?? throw new Exception("Invalid coupon");

            var discount = (dto.OriginalPrice * coupon.Percentage) / 100;

            var finalPrice = dto.OriginalPrice - discount;

            return finalPrice;
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
            
                IsActive = c.IsActive,
               
                
                ApplicablePlans = c.CouponSubscriptionPlans?
                    .Select(x => new SubscriptionPlanDto
                    {
                        Id = x.SubscriptionPlanId,
                        Name=x.SubscriptionPlan?.Name
                    }).ToList()
            };
        }
    }
}