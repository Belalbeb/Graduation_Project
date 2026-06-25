namespace Graduation_Project.Models
{
    public class CompanySubscription
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public Guid SubscriptionPlanId { get; set; }

        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;

        public BillingCycle BillingCycle { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal PaidAmount { get; set; }

        public bool IsActive { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    public enum BillingCycle
    {
        Monthly,
        Yearly
    }
}
