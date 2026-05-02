using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IResumeRepository
    {
        Task DeactivateAllAsync(int applicantId);
        Task<Resume> AddAsync(Resume resume);
        Task<string?> GetActivePathAsync(int applicantId);
        Task<string?> GetActivePathByUserIdAsync(string userId);
    }
}
