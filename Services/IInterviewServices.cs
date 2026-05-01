using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IInterviewServices
    {
        Task<InterviewStatisticsDto> GetStatisticsAsync(int applicantId);
        Task<List<InterviewResponseDto>> GetUpcomingAsync(int applicantId);
        Task<List<InterviewResponseDto>> GetCompletedAsync(int applicantId);
        Task<List<InterviewResponseDto>> GetCancelledAsync(int applicantId);
        Task<List<InterviewResponseDto>> GetAllAsync(int applicantId);
    }
}
