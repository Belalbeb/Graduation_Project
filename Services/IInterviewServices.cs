using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IInterviewServices
    {
        Task<List<Interview>?> GetUpcomingInterviewsAsync(int applicantId) ;
        Task<List<Interview>?> GetCompletedInterviewsAsync(int applicantId) ;
        Task<List<Interview>?> GetCancelledInterviewsAsync(int applicantId) ;
    }
}
