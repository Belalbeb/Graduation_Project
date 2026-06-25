namespace Graduation_Project.Dtos
{
    public class ApplicantDetailsForApplicationDto
    {
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public List<ResumeDetails> Resumes { get; set; }
    }
    public class ResumeDetails
    {
        public Guid ResumeId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}
