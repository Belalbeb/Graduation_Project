using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class JobPostingService : IJobPostingService
    {
        private readonly IJobPostingRepository _repository;

        public JobPostingService(IJobPostingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JobPosting>> GetAllJobsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<JobPosting> GetJobByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<JobPosting>> GetJobsByCompanyAsync(int companyId)
        {
            return await _repository.GetByCompanyAsync(companyId);
        }

        public async Task<JobPosting> CreateJobAsync(JobPosting jobPosting)
        {
            return await _repository.AddAsync(jobPosting);
        }

        public async Task<JobPosting> UpdateJobAsync(int id, JobPosting jobPosting)
        {
            return await _repository.UpdateAsync(id, jobPosting);
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> DeactivateJobAsync(int id)
        {
            return await _repository.DeactivateAsync(id);
        }
    }
}
