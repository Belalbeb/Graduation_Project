namespace Graduation_Project.Models
{
    public class Experience
    {
        public Guid ExperienceID { get; set; } = Guid.NewGuid();
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public JobType JobType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ?EndDate { get; set; }

        // FK
        public Guid ApplicantID { get; set; }
        public Applicant Applicant { get; set; }
    }
}
