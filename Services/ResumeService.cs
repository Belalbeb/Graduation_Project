using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ResumeService : IResumeServices
    {
        private readonly IResumeRepository _repository;

        public ResumeService(IResumeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Resume> UploadNewResumeAsync(int applicantId, string fileName, string filePath)
        {
            await _repository.DeactivateAllAsync(applicantId);

            var newResume = new Resume
            {
                FileName    = fileName,
                FilePath    = filePath,
                UploadDate  = DateTime.UtcNow,
                IsActive    = true,
                ApplicantID = applicantId
            };

            return await _repository.AddAsync(newResume);
        }

        public async Task<string?> GetActiveResumeAsync(int applicantId)
        {
            return await _repository.GetActivePathAsync(applicantId);
        }

        public async Task<string?> GetActiveResumePathByUserIdAsync(string userId)
        {
            return await _repository.GetActivePathByUserIdAsync(userId);
        }
    }
}
