using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface IProjectService
    {
        Task<List<ProjectResponseDto>> GetAllAsync(Guid applicantId);
        Task<ProjectResponseDto?> GetByIdAsync(Guid projectId,Guid applicantId);
        Task<ProjectResponseDto?> AddAsync(Guid applicantId,ProjectDto dto);
        Task<bool> UpdateAsync(Guid projectId,Guid applicantId,ProjectDto dto);
        Task<bool> DeleteAsync(Guid projectId,Guid applicantId);
    }
}
