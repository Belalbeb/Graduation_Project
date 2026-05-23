using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface IAdminServices
    {
        Task<AdminDashboardResponseDto> GetAdminDashboardAsync();
        Task<AdminJobOverviewDto> GetAdminJobDashboard();
        Task<AdminJobDetailsDto> AdminJobDetails(Guid jobId);
        Task<bool> AcceptJobAsync(Guid jobId);
         Task<bool> RejectJobAsync(Guid jobId);
    }
}
