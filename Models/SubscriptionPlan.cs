namespace Graduation_Project.Models
{
    public class SubscriptionPlan
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;

        // Pricing
        public decimal MonthlyPrice { get; set; }

        public decimal YearlyPrice { get; set; }

        // Limits
        public int MaxJobPostsPerMonth { get; set; }

        public int FeaturedJobPostsPerMonth { get; set; }

        // Features
        public bool HasAiToolsAccess { get; set; }

        public bool HasCandidateSearch { get; set; }

        public bool HasPrioritySupport { get; set; }

        // State
        public bool IsPublished { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<CompanySubscription> Subscriptions { get; set; }
            = new List<CompanySubscription>();
    }
}
