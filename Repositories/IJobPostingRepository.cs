using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IJobPostingRepository
    {
        Task<IEnumerable<JobPosting>> GetAllAsync();
        Task<JobPosting?> GetByIdAsync(Guid id);
        Task<IEnumerable<JobPosting>> GetByCompanyAsync(Guid companyId);
        Task<JobPosting> AddAsync(JobPosting jobPosting);
        Task<JobPosting?> UpdateAsync(Guid id, JobPosting jobPosting);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeactivateAsync(Guid id);
    }
}
