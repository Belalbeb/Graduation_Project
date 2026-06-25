using Graduation_Project.Models;

namespace Graduation_Project.Dtos
{
    public class CompanySubscriptionPageDto
    {
        public CurrentPlanDetailsDto CurrentPlan { get; set; } = null!;
        public List<AvailablePlanDto> AvailablePlans { get; set; } = [];
        public BillingHistoryDto BillingHistory { get; set; } = null!;
    }


    public class CurrentPlanDetailsDto
    {
        public Guid SubscriptionId { get; set; }
        public string PlanName { get; set; } = null!;
        public string? BillingCycle { get; set; }   // Monthly | Annually
        public DateTime? RenewalDate { get; set; }
        public string Status { get; set; } = null!; // "Active" | "Expired" | "Cancelled"

        // Progress bars
        public UsageItem ActiveJobs { get; set; } = null!;
        public UsageItem FeaturedPosts { get; set; } = null!;
        public UsageItem SubscriptionProgress { get; set; } = null!;
    }

    public class UsageItem
    {
        public int Used { get; set; }
        public int Limit { get; set; }

        /// <summary>0–100 for the progress bar width.</summary>
      
    }



    public class AvailablePlanDto
    {
        public Guid PlanId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        // Prices for both billing cycles so the frontend toggle works
        // without a second API call.
        public decimal MonthlyPrice { get; set; }
        public decimal YearlyPrice { get; set; }

        // Features (green ✔ / red ✘ icons)
        public int MaxActiveJobs { get; set; }
        public int MaxFeaturedJobs { get; set; }
        public bool HasAiToolsAccess { get; set; }
        public bool HasStandardSupport { get; set; }
        public bool HasCandidateSearch { get; set; }

        /// <summary>True when this is the company's currently active plan.</summary>
        public bool IsCurrentPlan { get; set; }
    }



    public class BillingHistoryDto
    {
        public int TotalTransactions { get; set; }
        public List<BillingRecordDto> Records { get; set; } = [];
    }

    public class BillingRecordDto
    {
        public Guid SubscriptionId { get; set; }
        public string PlanName { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ?EndDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
