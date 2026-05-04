using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IJobPostingService
    {
        Task<IEnumerable<JobPosting>> GetAllJobsAsync();
        Task<JobPosting> GetJobByIdAsync(int id);
        Task <JobPostingDto> GetJobsByCompanyAsync(int companyId);
        Task<JobPosting> CreateJobAsync(CreateJobDto dto,int companyId);
        Task<JobPosting> UpdateJobAsync(int id, JobPosting jobPosting);
        Task<bool> DeleteJobAsync(int id);
        Task<bool> DeactivateJobAsync(int id);
    }
}
