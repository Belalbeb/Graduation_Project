using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> AddAsync(Company company);
        Task<Company?> GetByIdAsync(Guid id);
        Task<Company?> GetByUserIdAsync(string userId);
        Task<Company?> GetWithJobPostingsAndApplicationsAsync(Guid companyId);
        Task<bool> UpdateAsync(Company company);
        Task<bool> DeleteAsync(Guid id);

        Task<Company?> GetCompanyForSettingsAsync(Guid companyId);
        Task<bool> UpdateCompanyProfileAsync(Company company);
        Task<List<JobPosting>> GetJobPostingsByCompanyIdAsync(Guid companyId);
        Task<int> GetCompaniesCount();

        // ======================== Admin ==========================

        Task<List<Company>> GetAllCompaniesForAdminAsync();
        Task<Company?> GetCompanyWithDetailsForAdminAsync(Guid companyId);
        Task<int> CountTotalCompaniesAsync();
        Task<int> CountVerifiedCompaniesAsync();
        Task<int> CountPendingCompaniesAsync();
        Task<int> CountCompanyJobsAsync(Guid companyId);
        Task<int> CountCompanyActiveJobsAsync(Guid companyId);
        Task<int> CountCompanyApplicantsAsync(Guid companyId);
        Task<int> CountCompanyInterviewsAsync(Guid companyId);
        Task<string?> GetCompanyActiveSubscriptionPlanAsync(Guid companyId);
    }
}