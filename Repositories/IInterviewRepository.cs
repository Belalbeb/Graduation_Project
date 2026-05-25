using Graduation_Project.Dtos;
using Graduation_Project.Dtos.Company.Interview;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IInterviewRepository
    {
        // Applicant side methods
        Task<int> CountByApplicantAsync(Guid applicantId);
        Task<int> CountByApplicantAndStatusAsync(Guid applicantId,InterviewStatus status);
        Task<List<Interview>> GetByApplicantAndStatusAsync(Guid applicantId,InterviewStatus status);
        Task<List<Interview>> GetAllByApplicantAsync(Guid applicantId);

        Task<CompanyInterviewStatisticsDto> GetCompanyStatisticsAsync(Guid companyId);
        Task<List<Interview>> GetCompanyInterviewsAsync(Guid companyId,string? search = null,string? status = null);
        Task<Interview?> GetCompanyInterviewByIdAsync(Guid interviewId,Guid companyId);
        Task UpdateAsync(Interview interview);

        Task<List<Interview>> GetByjobPostingId(Guid jobId);
        Task<Interview> GetInterviewById(Guid InterviewId);
        Task<bool> ChangeInterviewDate(Guid InterviewId,DateTime InterviewDate);
    }
}