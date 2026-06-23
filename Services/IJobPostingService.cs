using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IJobPostingService
    {
         Task<PagedResult<JobCardDto>> GetAllJobsAsync(Guid currentApplicantId, JobFilterDto jobFilterDto);
        Task<JobPosting> GetJobByIdAsync(Guid id);
        Task <JobPostingDto> GetJobsByCompanyAsync(Guid companyId);
        Task<JobPosting> CreateJobAsync(CreateJobDto dto,Guid companyId);
        Task<bool> UpdateJobAsync(Guid id, UpdateJobDto jobPosting);
        Task<bool> DeleteJobAsync(Guid id);
        Task<bool> DeactivateJobAsync(Guid id);
        public Task<JobInformationResponseDto> JobDetails(Guid jobId);
    }
}
