namespace Graduation_Project.Models
{
    public class Experience
    {
        public int ExperienceID { get; set; }
        public string CompanyName { get; set; }
        public string Location { get ; set ;}
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public JobType JobType { get ; set ;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        // FK
        public int ApplicantID { get; set; }
        public Applicant Applicant { get; set; }
    }
}
