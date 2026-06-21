namespace Graduation_Project.Models
{
    public class VerificationDocument
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid VerificationRequestId { get; set; }

        public CompanyVerificationRequest VerificationRequest { get; set; }
            = null!;

        public string FileUrl { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
        public float? FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
            = DateTime.UtcNow;
    }
}
