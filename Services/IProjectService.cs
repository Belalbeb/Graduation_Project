using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface IProjectService
    {
        Task<List<ProjectResponseDto>> GetAllAsync(int applicantId);
        Task<ProjectResponseDto?> GetByIdAsync(int projectId,int applicantId);
        Task<ProjectResponseDto?> AddAsync(int applicantId,ProjectDto dto);
        Task<bool> UpdateAsync(int projectId,int applicantId,ProjectDto dto);
        Task<bool> DeleteAsync(int projectId,int applicantId);
    }
}
