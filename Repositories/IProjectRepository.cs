using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllByApplicantAsync(int applicantId);
        Task<Project?> GetByIdAsync(int projectId, int applicantId);
        Task<Project> AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}
