using System.ComponentModel.DataAnnotations;
using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class CreateCouponDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z0-9_\-]+$")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100)]
        public decimal Percentage { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalUsageLimit { get; set; }

        public bool IsActive { get; set; } = true;

        public List<Coupon.ApplicablePlan>? ApplicablePlans { get; set; }

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

        public List<Coupon.ApplicablePlan>? ApplicablePlans { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }

    public class QueryCouponDto
    {
        public string? Search { get; set; }
        public Coupon.CouponStatus? Status { get; set; }
        public decimal? Percentage { get; set; }
        public Coupon.ApplicablePlan? Plan { get; set; }

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
        public Coupon.ApplicablePlan Plan { get; set; }
    }

    public class ApplyCouponDto : ValidateCouponDto
    {
        [Required]
        public string SubscriptionId { get; set; } = string.Empty;
    }

    public class CouponResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
        public int TotalUsageLimit { get; set; }
        public int UsedCount { get; set; }
        public int RemainingUses { get; set; }
        public bool IsActive { get; set; }
        public Coupon.CouponStatus Status { get; set; }
        public List<Coupon.ApplicablePlan>? ApplicablePlans { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CouponValidationResult
    {
        public bool Valid { get; set; }
        public CouponResponseDto Coupon { get; set; } = null!;
        public decimal DiscountAmount { get; set; }
    }

    public class GeneratedCodeDto
    {
        public string Code { get; set; } = string.Empty;
    }

    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalPages { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null)
            => new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string message)
            => new() { Success = false, Message = message };
    }
}