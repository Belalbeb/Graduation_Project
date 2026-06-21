namespace Graduation_Project.Models
{
    public class CompanyVerificationRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public VerificationStatus Status { get; set; }
            = VerificationStatus.Pending;

        public string? AdminNotes { get; set; }

        public DateTime SubmittedAt { get; set; }
            = DateTime.UtcNow;

       

        public ICollection<VerificationDocument> Documents { get; set; }
            = new List<VerificationDocument>();
    }
    public enum VerificationStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        NeedsMoreInformation = 3
    }
}
