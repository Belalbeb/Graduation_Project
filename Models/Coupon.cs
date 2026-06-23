namespace Graduation_Project.Models
{
    public class Coupon
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; } = string.Empty;

        public decimal Percentage { get; set; }

      
        public int TotalUsageLimit { get; set; }

        public int UsedCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

   
        public ICollection<CouponSubscriptionPlan> CouponSubscriptionPlans { get; set; }
    = new List<CouponSubscriptionPlan>();

        public DateTime? ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }

        
        public bool IsDeleted => DeletedAt.HasValue;

        public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;

        public int RemainingUses => TotalUsageLimit - UsedCount;

        public CouponStatus Status
        {
            get
            {
                if (IsExpired) return CouponStatus.Expired;
                if (!IsActive) return CouponStatus.Inactive;
                return CouponStatus.Active;
            }
        }
     

        public enum CouponStatus
        {
            Active,
            Inactive,
            Expired
        }


    }
}