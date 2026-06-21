using Graduation_Project.Dtos;
using Graduation_Project.Dtos.Company.Interview;

namespace Graduation_Project.Services
{
    public interface ICompanyInterviewService
    {
        Task<CompanyInterviewStatisticsDto> GetStatisticsAsync(Guid companyId);
        Task<List<CompanyInterviewListDto>> GetInterviewsAsync(Guid companyId,string? search = null,string? status = null);
        Task<CompanyInterviewDetailsDto?> GetInterviewDetailsAsync(Guid interviewId,Guid companyId);
        Task<bool> UpdateInterviewAsync(Guid interviewId, Guid companyId, UpdateCompanyInterviewDto dto);
        Task<Guid> ScheduleInterviewAsync(ScheduleInterviewDto dto);
      
    }
}
