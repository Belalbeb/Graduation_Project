using Graduation_Project.Dtos;
using Graduation_Project.Dtos.Admin.Applicant;
using Graduation_Project.Dtos.Admin.Company;

namespace Graduation_Project.Services
{
    public interface IAdminServices
    {
        Task<AdminDashboardResponseDto> GetAdminDashboardAsync();
        Task<AdminJobOverviewDto> GetAdminJobDashboard();
        Task<AdminJobDetailsDto> AdminJobDetails(Guid jobId);
        Task<bool> AcceptJobAsync(Guid jobId);
         Task<bool> RejectJobAsync(Guid jobId);

        // ======================= Applicants ======================
        Task<AdminUserStatsDto> GetUserStatsAsync();
        Task<List<AdminUserListDto>> GetAllApplicantsAsync();
        Task<AdminUserDetailsDto?> GetApplicantDetailsAsync(Guid applicantId);
        Task<bool> BlockApplicantAsync(Guid applicantId);
        Task<bool> UnblockApplicantAsync(Guid applicantId);
   
        Task<bool> DeleteApplicantAsync(Guid applicantId);

        // ================= Company ====================

        Task<AdminCompanyStatsDto> GetCompanyStatsAsync();
        Task<List<AdminCompanyListDto>> GetAllCompaniesAsync();
        Task<AdminCompanyDetailsDto?> GetCompanyDetailsAsync(Guid companyId);
        Task<bool> VerifyCompanyAsync(Guid companyId);
        Task<bool> BlockCompanyAsync(Guid companyId);
        Task<bool> UnblockCompanyAsync(Guid companyId);
        Task<bool> DeleteCompanyAsync(Guid companyId);
    }
}
