namespace Graduation_Project.Models
{
    public class Resume
    {
        public int ResumeID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsActive { get; set; }

        // FK
        public int ApplicantID { get; set; }
        public Applicant Applicant { get; set; }

        // Navigation
        public ICollection<Application> Applications { get; set; }
    }
}
