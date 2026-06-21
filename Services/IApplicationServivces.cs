using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IApplicationServivces
    {
        Task<List<ApplicationDto>> GetApplicationByApplicant(Guid id);
        public Task<ApplicantCompanyDto> GetApplicantByApplication(Guid ApplicationId);
        public Task<bool> ChangeApplicationStatus(Guid ApplicationId, Guid companyId, ApplicationStatus status);
        public Task<Application> CreateApplication(Guid ApplicantId, CreateApplicationDto createApplicationDto);
    }
}
