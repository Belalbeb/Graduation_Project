using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IResumeServices
    {
        Task<Resume> UploadNewResumeAsync(Guid applicantId, string fileName, string filePath) ;
        Task<string?> GetActiveResumeAsync(Guid applicantId) ;
        Task<string?> GetActiveResumePathByUserIdAsync(string userId) ;
    }
}