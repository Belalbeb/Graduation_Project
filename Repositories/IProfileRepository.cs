using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IProfileRepository
    {
        Task<Applicant?> GetApplicantByUserIdAsync(string userId);
        Task<Company?> GetCompanyByUserIdAsync(string userId);
        Task<Applicant?> GetPublicProfileAsync(int applicantId);
    }
}
