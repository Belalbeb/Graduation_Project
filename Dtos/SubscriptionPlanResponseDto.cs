namespace Graduation_Project.Dtos
{
    public class SubscriptionPlanResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;
        public int NumberOfUser { get; set; }

        public decimal MonthlyPrice { get; set; }

        public decimal YearlyPrice { get; set; }

    
        public int MaxJobPostsPerMonth { get; set; }

        public int FeaturedJobPostsPerMonth { get; set; }


        public bool HasAiToolsAccess { get; set; }

        public bool HasCandidateSearch { get; set; }

        public bool HasPrioritySupport { get; set; }

 
        public bool IsPublished { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
