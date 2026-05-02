using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ISettingsRepository
    {
        Task<Applicant?> GetApplicantByIdAsync(int applicantId);
        Task<Resume?> GetActiveResumeAsync(int applicantId);
        Task UpdateApplicantAsync(Applicant applicant);
        Task DeactivateAllResumesAsync(int applicantId);
        Task AddResumeAsync(Resume resume);
    }
}
