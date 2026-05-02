using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IExperienceRepository
    {
        Task<Experience?> GetByIdAsync(int experienceId);
        Task<List<Experience>> GetAllByApplicantAsync(int applicantId);
        Task<bool> HasOverlappingExperienceAsync(Experience experience);
        Task<Experience> AddAsync(Experience experience);
        Task UpdateAsync(Experience experience);
        Task DeleteAsync(Experience experience);
    }
}
