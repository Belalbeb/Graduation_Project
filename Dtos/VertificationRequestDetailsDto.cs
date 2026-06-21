using Microsoft.AspNetCore.Http.HttpResults;

namespace Graduation_Project.Dtos
{
    public class VertificationRequestDetailsDto
    {
        public Guid CompanyId { get; set; }
        public string ?CompanyName { get; set; }
        public string? CompanyCoverImage { get; set; }
        public string ?CompanyLogoImage { get; set; }
        public string ?Status { get; set; }
        public string ?Notes { get; set; }
        public string? CompanyDescription { get; set; }
        public List<VerificationDocumentDto> Documents { get; set; }

        public DateTime CreatedAt { get; set; }

    }
    public class VerificationDocumentDto
    {
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public float ?FileSize { get; set; }
    }
}
