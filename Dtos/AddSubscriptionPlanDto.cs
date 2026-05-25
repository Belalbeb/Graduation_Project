using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class AddSubscriptionPlanDto : IValidatableObject
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string ShortDescription { get; set; } = null!;

        // Pricing

        [Range(0, 100000)]
        public decimal MonthlyPrice { get; set; }

        [Range(0, 100000)]
        public decimal YearlyPrice { get; set; }

        // Limits

        [Range(0, 10000)]
        public int MaxJobPostsPerMonth { get; set; }

        [Range(0, 10000)]
        public int FeaturedJobPostsPerMonth { get; set; }

        // Features

        public bool HasAiToolsAccess { get; set; }

        public bool HasCandidateSearch { get; set; }

        public bool HasPrioritySupport { get; set; }

        // State

        public bool IsPublished { get; set; } = true;


        // Custom Validation

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (YearlyPrice < MonthlyPrice)
            {
                yield return new ValidationResult(
                    "Yearly price must be greater than or equal to monthly price.",
                    new[] { nameof(YearlyPrice) });
            }

            if (FeaturedJobPostsPerMonth > MaxJobPostsPerMonth)
            {
                yield return new ValidationResult(
                    "Featured job posts cannot exceed max job posts.",
                    new[] { nameof(FeaturedJobPostsPerMonth) });
            }
        }
    }
}