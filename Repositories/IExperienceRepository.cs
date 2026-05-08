using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IExperienceRepository
    {
        Task<Experience?> GetByIdAsync(Guid experienceId);
        Task<List<Experience>> GetAllByApplicantAsync(Guid applicantId);
        Task<bool> HasOverlappingExperienceAsync(Experience experience);
        Task<Experience> AddAsync(Experience experience);
        Task UpdateAsync(Experience experience);
        Task DeleteAsync(Experience experience);
    }
}
