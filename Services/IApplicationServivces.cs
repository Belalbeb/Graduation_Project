using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IApplicationServivces
    {
        Task<List<ApplicationDto>> GetApplicationByApplicant(Guid id);
    }
}
