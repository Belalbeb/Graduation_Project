using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllByApplicantAsync(Guid applicantId);
        Task<Project?> GetByIdAsync(Guid projectId, Guid applicantId);
        Task<Project> AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}
