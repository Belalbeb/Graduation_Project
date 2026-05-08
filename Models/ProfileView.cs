namespace Graduation_Project.Models
{
    public class ProfileView
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
    }
}
