using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IApplicationRepository
    {
        Task<List<ApplicationDto>> GetByApplicantIdAsync(Guid applicantId);
        public Task<Application> GetApplicationByIdAsync(Guid ApplicationId);
        public  Task<bool> ChangeApplicationStatus(Guid ApplicationId, ApplicationStatus status);
    }
}
