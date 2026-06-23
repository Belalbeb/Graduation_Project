namespace Graduation_Project.Models
{
    public class CouponSubscriptionPlan
    {
        public Guid Id { get; set; }
        public Guid CouponId { get; set; }

        public Coupon Coupon { get; set; } = null!;

        public Guid SubscriptionPlanId { get; set; }

        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;
    }
}
