using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IResumeServices
    {
        Task<Resume> UploadNewResumeAsync(int applicantId, string fileName, string filePath) ;
        Task<string?> GetActiveResumeAsync(int applicantId) ;
        Task<string?> GetActiveResumePathByUserIdAsync(string userId) ;
    }
}