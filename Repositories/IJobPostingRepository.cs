using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IJobPostingRepository
    {
        Task<IEnumerable<JobPosting>> GetAllAsync();
        Task<JobPosting?> GetByIdAsync(int id);
        Task<IEnumerable<JobPosting>> GetByCompanyAsync(int companyId);
        Task<JobPosting> AddAsync(JobPosting jobPosting);
        Task<JobPosting?> UpdateAsync(int id, JobPosting jobPosting);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeactivateAsync(int id);
    }
}
