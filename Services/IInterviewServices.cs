using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IInterviewServices
    {
        Task<InterviewStatisticsDto> GetStatisticsAsync(Guid applicantId);
        Task<List<InterviewResponseDto>> GetUpcomingAsync(Guid applicantId);
        Task<List<InterviewResponseDto>> GetCompletedAsync(Guid applicantId);
        Task<List<InterviewResponseDto>> GetCancelledAsync(Guid applicantId);
        Task<List<InterviewResponseDto>> GetAllAsync(Guid applicantId);
        public Task<InterviewCompanyResponseDto> InterviewCompanyDetails(Guid InterviewId);
        public  Task<bool> ChangeInterviewDate(Guid InterviewId,DateOnly InterviewDate);
       
    }
}
