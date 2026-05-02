using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IInterviewRepository
    {
        Task<int> CountByApplicantAsync(int applicantId);
        Task<int> CountByApplicantAndStatusAsync(int applicantId, InterviewStatus status);
        Task<List<Interview>> GetByApplicantAndStatusAsync(int applicantId, InterviewStatus status);
        Task<List<Interview>> GetAllByApplicantAsync(int applicantId);
    }
}
