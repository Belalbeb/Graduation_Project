namespace Graduation_Project.Dtos
{
    public class SubscriptionDashboardDto
    {
        public int TotalSubscribers { get; set; }

        public int NewSubscriptions { get; set; }

        public int ActiveSubscriptions { get; set; }

        public int CancelledSubscriptions { get; set; }
        public List<SubscriptionPlanResponseDto> Plans { get; set; }
    }
}
