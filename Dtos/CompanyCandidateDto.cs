using System.Security.Principal;

namespace Graduation_Project.Dtos
{
    public class CompanyCandidateDto
    {
        public List<CandidateDetailsDto>  candidates { get; set; }
        public int totalCandidates { get; set; }
    }
    public class CandidateDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Industry { get; set; }
        public string JobTitle { get; set; }
        public string Country { get; set; }

    }
}
