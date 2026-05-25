using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class CreateSubscriptionDto
    {
        public Guid SubscriptionPlanId { get; set; }

        public BillingCycle BillingCycle { get; set; }
    }
}
