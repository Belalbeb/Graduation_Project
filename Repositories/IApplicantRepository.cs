using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IApplicantRepository
    {
        Task<Applicant> CreateAsync(Applicant applicant);
        Task<Applicant?> GetByIdAsync(int id);
        Task<Applicant?> GetByUserIdAsync(string userId);
        Task<List<Applicant>> GetAllAsync();
        Task UpdateAsync(Applicant applicant);
        Task DeleteAsync(Applicant applicant);

        // Dashboard queries
        Task<int> CountApplicationsAsync(int applicantId);
        Task<int> CountSavedJobsAsync(int applicantId);
        Task<int> CountUpcomingInterviewsAsync(int applicantId);
        Task<int> CountProfileViewsAsync(int applicantId);
        Task<List<dynamic>> GetMonthlyApplicationsAsync(int applicantId, DateTime startOfYear);
        Task<List<dynamic>> GetMonthlyInterviewsAsync(int applicantId, DateTime startOfYear);
        Task<List<RecentApplicationDto>> GetRecentApplicationsAsync(int applicantId, int take);

        // Saved jobs
        Task<List<SavedJobsResponseDto>> GetSavedJobsAsync(int applicantId);

        // Resume
        Task DeactivateAllResumesAsync(int applicantId);
        Task AddResumeAsync(Resume resume);
        Task<string?> GetActiveResumePathAsync(int applicantId);
        Task<string?> GetActiveResumePathByUserIdAsync(string userId);
    }
}
