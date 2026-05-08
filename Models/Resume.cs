namespace Graduation_Project.Models
{
    public class Resume
    {
        public Guid ResumeID { get; set; } = Guid.NewGuid();
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsActive { get; set; } = true;

        // FK
        public Guid ApplicantID { get; set; }
        public Applicant Applicant { get; set; }

        // Navigation
        public ICollection<Application> Applications { get; set; }
    }
}
