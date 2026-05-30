namespace Graduation_Project.Dtos.Admin.Applicant
{
    public class AdminUserListDto
    {
        public Guid ApplicantId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }
    }
}
