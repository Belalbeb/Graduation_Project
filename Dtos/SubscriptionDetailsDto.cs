using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class SubscriptionDetailsDto
    {
        public Guid Id { get; set; }

        public string CompanyName { get; set; } = null!;

        public string PlanName { get; set; } = null!;

        public BillingCycle BillingCycle { get; set; }

        public decimal PaidAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public bool IsCancelled { get; set; }

    }
}