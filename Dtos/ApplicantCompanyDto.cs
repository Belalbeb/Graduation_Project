namespace Graduation_Project.Dtos
{
    public class ApplicantCompanyDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid ApplicantId { get; set; }
        public string ImageUrl { get; set; }
        public string CvPath { get; set; }
        public string CVName { get; set; }
        public string ApplicationStatus { get; set; }
       
    }
}
