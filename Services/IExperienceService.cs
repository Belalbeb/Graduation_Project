using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IExperienceService
    {
        Task<Experience?> GetExperienceByIdAsync(Guid experienceId) ;
        Task<List<ExperienceResponseDto>> GetAllAsync(Guid applicantId) ;
        Task<ExperienceResponseDto?> AddExperienceAsync(Experience experience) ;
        Task<int> UpdateExperienceAsync(Guid experienceId, ExperienceDto experienceDto) ;
        Task<bool> DeleteExperienceAsync(Guid experienceId) ;
    }
}
