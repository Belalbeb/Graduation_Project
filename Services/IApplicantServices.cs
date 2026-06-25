using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IApplicantServices
    {
        Task<Applicant> CreateApplicantAsync(Applicant applicant);
        Task<List<Applicant>> GetAllApplicantAsync();
        public Task<Applicant> GetApplicantByIdAsync(Guid id);
        Task<bool> UpdateApplicantAsync(Guid id, ApplicantDto applicant);
        Task<bool> DeleteApplicantAsync(Guid id);
        Task<ApplicantDashboardResponseDto> GetDashboardAsync(Guid applicantId);
        Task<List<SavedJobsResponseDto>> GetSavedsAsync(Guid id);
        Task<Resume> UploadResumeAsync(Guid applicantId,string fileName,string filePath);
        Task<string> GetActiveResumePathAsync(Guid applicantId);
        Task<string> GetActiveResumePathByUserIdAsync(string userId);
        public Task<Applicant> GetApplicantByUserIdAsync(string userId);
    Task<ApplicantDetailsForApplicationDto> GetApplicantDetailsForApplication(Guid ApplicantId);
    }
}
