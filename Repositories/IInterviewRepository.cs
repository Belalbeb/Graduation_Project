using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IInterviewRepository
    {
        Task<int> CountByApplicantAsync(Guid applicantId);
        Task<int> CountByApplicantAndStatusAsync(Guid applicantId, InterviewStatus status);
        Task<List<Interview>> GetByApplicantAndStatusAsync(Guid applicantId, InterviewStatus status);
        Task<List<Interview>> GetAllByApplicantAsync(Guid applicantId);
        public Task<List<Interview>> GetByjobPostingId(Guid jobId);
        public Task<Interview> GetInterviewById(Guid InterviewId);
        public Task<bool> ChangeInterviewDate(Guid InterviewId,DateOnly InterviewDate);
    }
}
