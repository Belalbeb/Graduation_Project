using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ISavedJobRepository
    {
        Task AddAsync(SavedJobs savedJob);
        Task<List<SavedJobs>> GetSavedJobsAsync(
         Guid applicantId);
        Task UnsaveJobAsync(Guid applicantId, Guid jobId);

        Task<bool> ExistsAsync(Guid applicantId, Guid jobId);
    }
}
