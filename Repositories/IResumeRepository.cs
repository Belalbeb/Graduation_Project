using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IResumeRepository
    {
        Task DeactivateAllAsync(Guid applicantId);
        Task<Resume> AddAsync(Resume resume);
        Task<string?> GetActivePathAsync(Guid applicantId);
        Task<string?> GetActivePathByUserIdAsync(string userId);
    }
}
