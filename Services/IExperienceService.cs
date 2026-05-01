using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IExperienceService
    {
        Task<Experience?> GetExperienceByIdAsync(int experienceId) ;
        Task<List<ExperienceResponseDto>> GetAllAsync(int applicantId) ;
        Task<ExperienceResponseDto?> AddExperienceAsync(Experience experience) ;
        Task<int> UpdateExperienceAsync(int experienceId, ExperienceDto experienceDto) ;
        Task<bool> DeleteExperienceAsync(int experienceId) ;
    }
}
