using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface ISavedJobService
    {
        Task SaveJobAsync(Guid applicantId, Guid jobId);
        Task<List<JobCardDto>> GetSavedJobsAsync(Guid applicantId);
        Task UnsaveJobAsync(Guid applicantId, Guid jobId);
    }
}
