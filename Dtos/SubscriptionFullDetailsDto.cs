using Graduation_Project.Models;

namespace Graduation_Project.DTOs.Subscriptions
{
 

    public class SubscriptionFullDetailsDto
    {
        public CompanyInfoDto Company { get; set; } = null!;
        public CurrentSubscriptionDto CurrentSubscription { get; set; } = null!;
        public PlanUsageDto PlanUsage { get; set; } = null!;
        public PlanFeaturesDto AllowedFeatures { get; set; } = null!;
        public SubscriptionHistoryDto SubscriptionHistory { get; set; } = null!;
    }



    public class CompanyInfoDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string CompanyEmail { get; set; } = null!;
        public string Industry { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime JoinedAt { get; set; }
        public string? CompanyLogoUrl { get; set; }
    }

 

    public class CurrentSubscriptionDto
    {
        public Guid SubscriptionId { get; set; }
        public string PlanName { get; set; } = null!;
        public string PlanDescription { get; set; } = null!;
        public decimal Price { get; set; }
        public string? BillingCycle { get; set; }   // Monthly | Annually
        public DateTime StartDate { get; set; }
        public DateTime ?EndDate { get; set; }   // = Renewal Date on UI
        public int DaysLeft { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = null!; // "Active" | "Expired" | "Pending" | "Cancelled"
    }



    public class PlanUsageDto
    {
        public UsageItemDto ActiveJobs { get; set; } = null!;
        public UsageItemDto FeaturedPosts { get; set; } = null!;
        public UsageItemDto SubscriptionProgress { get; set; } = null!;
    }

    public class UsageItemDto
    {
        public int Used { get; set; }
        public int Limit { get; set; }

        /// <summary>0–100 percentage for the progress bar.</summary>
        public double Percentage => Limit > 0
            ? Math.Round((double)Used / Limit * 100, 1)
            : 0;
    }


    public class PlanFeaturesDto
    {
        public int ActiveJobPostsLimit { get; set; }
        public int FeaturedJobsLimit { get; set; }
        public bool HasAiToolsAccess { get; set; }
        public bool HasStandardSupport { get; set; }
        public bool HasPrioritySupport { get; set; }
        public bool HasCandidateSearch { get; set; }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  5. SUBSCRIPTION HISTORY  (bottom table)
    // ═══════════════════════════════════════════════════════════════════

    public class SubscriptionHistoryDto
    {
        public int TotalSubscriptions { get; set; }
        public List<SubscriptionRecordDto> Records { get; set; } = [];
    }

    public class SubscriptionRecordDto
    {
      
        public Guid Id { get; set; }
        public string PlanName { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime BillingDate { get; set; }
        public string Status { get; set; } = null!; // "Active" | "Expired" |  "Cancelled"
    }
}