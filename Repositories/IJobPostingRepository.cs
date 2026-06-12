using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IJobPostingRepository
    {
        Task<IEnumerable<JobPosting>> GetAllAsync();
        Task<JobPosting?> GetByIdAsync(Guid id);
        Task<IEnumerable<JobPosting>> GetByCompanyAsync(Guid companyId);
        Task<JobPosting> AddAsync(JobPosting jobPosting);
        Task<bool> UpdateAsync( JobPosting jobPosting);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeactivateAsync(Guid id);
        Task<int> GetActiveJobsCountAsync();
        Task<int> GetPendingJobsCountAsync();
        Task<List<JobPosting>> GetLatestJobsAsync(int limit);
       Task<List<JobPosting>> GetPendingApprovalsAsync();
       Task<List<MonthlyStats>> GetMonthlyStatsAsync();
         Task<int> TotalJobs();
        Task<int> GetRejectedJobsCount();
        Task<bool> AcceptJobAsync(Guid jobId);
        Task<bool> RejectJobAsync(Guid jobId);
        public Task ReplaceSkillsAsync(Guid jobId, List<string> skills);

    }
}
