using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IApplicantRepository
    {
        Task<Applicant> CreateAsync(Applicant applicant);
        Task<Applicant?> GetByIdAsync(Guid id);
        Task<Applicant?> GetByUserIdAsync(string userId);
        Task<List<Applicant>> GetAllAsync();
       Task<(List<Applicant> Items, int TotalCount)> GetCandidatesAsync(
int page,
CandidateFilterDto filter);
        Task UpdateAsync(Applicant applicant);
        Task DeleteAsync(Applicant applicant);

        // Dashboard queries
        Task<int> CountApplicationsAsync(Guid applicantId);
        Task<int> CountSavedJobsAsync(Guid applicantId);
        Task<int> CountUpcomingInterviewsAsync(Guid applicantId);
        Task<int> CountProfileViewsAsync(Guid applicantId);
        Task<List<dynamic>> GetMonthlyApplicationsAsync(Guid applicantId, DateTime startOfYear);
        Task<List<dynamic>> GetMonthlyInterviewsAsync(Guid applicantId, DateTime startOfYear);
        Task<List<RecentApplicationDto>> GetRecentApplicationsAsync(Guid applicantId, int take);

        // Saved jobs
        Task<List<SavedJobsResponseDto>> GetSavedJobsAsync(Guid applicantId);

        // Resume
        Task DeactivateAllResumesAsync(Guid applicantId);
        Task AddResumeAsync(Resume resume);
        Task<string?> GetActiveResumePathAsync(Guid applicantId);
        Task<string?> GetActiveResumePathByUserIdAsync(string userId);
       Task<int> CountActiveApplicant();


        Task<List<Applicant>> GetAllApplicantsWithDetailsAsync();
        Task<Applicant?> GetApplicantWithAllDetailsAsync(Guid applicantId);
        Task<int> CountBlockedApplicantsAsync();
        Task<int> CountNewApplicantsThisMonthAsync();
    }
}
