using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class CreateSubscriptionDto
    {
        public Guid SubscriptionPlanId { get; set; }

        public string BillingCycle { get; set; }
        public string? CouponCode { get; set; }
    }
}
