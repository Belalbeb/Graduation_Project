using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
  
    public class CreateCouponDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
 
        public string Code { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100)]
        public decimal Percentage { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalUsageLimit { get; set; }

        public bool IsActive { get; set; } = true;

      

        public List<Guid>? SubscriptionPlanIds { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }

 
    public class UpdateCouponDto
    {
        public string? Code { get; set; }

        [Range(0.01, 100)]
        public decimal? Percentage { get; set; }

        [Range(1, int.MaxValue)]
        public int? TotalUsageLimit { get; set; }

        public bool? IsActive { get; set; }

   

        public List<Guid>? SubscriptionPlanIds { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }


    public class QueryCouponDto
    {
        public string? Search { get; set; }

        public Guid? SubscriptionPlanId { get; set; }

        public decimal? Percentage { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int Limit { get; set; } = 20;
    }


    public class ValidateCouponDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public Guid SubscriptionPlanId { get; set; }
    }

    public class ApplyCouponDto:ValidateCouponDto
    {
     
    }

    // ================= RESPONSE =================
    public class CouponResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
        public int TotalUsageLimit { get; set; }
        public int UsedCount { get; set; }
     
        public bool IsActive { get; set; }

  

        public List<SubscriptionPlanDto>? ApplicablePlans { get; set; }

       
    }

    public class SubscriptionPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

 
    public class CouponValidationResult
    {
        public bool IsValid { get; set; }
        //public CouponResponseDto Coupon { get; set; } = null!;
        public decimal DiscountAmount { get; set; }
    }
    public class CalculatePriceDto
    {
        public decimal OriginalPrice { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public Guid SubscriptionPlanId { get; set; }
    }




}