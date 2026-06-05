using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IJobPostingService
    {
        Task<IEnumerable<JobCardDto>> GetAllJobsAsync(Guid currentUserId);
        Task<JobPosting> GetJobByIdAsync(Guid id);
        Task <JobPostingDto> GetJobsByCompanyAsync(Guid companyId);
        Task<JobPosting> CreateJobAsync(CreateJobDto dto,Guid companyId);
        Task<JobPosting> UpdateJobAsync(Guid id, JobPosting jobPosting);
        Task<bool> DeleteJobAsync(Guid id);
        Task<bool> DeactivateJobAsync(Guid id);
        public Task<JobInformationResponseDto> JobDetails(Guid jobId);
    }
}
