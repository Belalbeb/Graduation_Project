using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IInterviewRepository
    {
        Task<int> CountByApplicantAsync(Guid applicantId);
        Task<int> CountByApplicantAndStatusAsync(Guid applicantId, InterviewStatus status);
        Task<List<Interview>> GetByApplicantAndStatusAsync(Guid applicantId, InterviewStatus status);
        Task<List<Interview>> GetAllByApplicantAsync(Guid applicantId);
    }
}
