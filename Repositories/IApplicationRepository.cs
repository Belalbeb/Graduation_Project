using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IApplicationRepository
    {
        Task<List<ApplicationDto>> GetByApplicantIdAsync(Guid applicantId);
    }
}
