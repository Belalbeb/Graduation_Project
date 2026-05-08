using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ISettingsRepository
    {
        Task<Applicant?> GetApplicantByIdAsync(Guid applicantId);
        Task<Resume?> GetActiveResumeAsync(Guid applicantId);
        Task UpdateApplicantAsync(Applicant applicant);
        Task DeactivateAllResumesAsync(Guid applicantId);
        Task AddResumeAsync(Resume resume);
    }
}
