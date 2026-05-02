using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProjectResponseDto>> GetAllAsync(int applicantId)
        {
            var projects = await _repository.GetAllByApplicantAsync(applicantId);
            return projects.Select(MapToDto).ToList();
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(int projectId, int applicantId)
        {
            var project = await _repository.GetByIdAsync(projectId, applicantId);
            return project == null ? null : MapToDto(project);
        }

        public async Task<ProjectResponseDto?> AddAsync(int applicantId, ProjectDto dto)
        {
            var project = new Project
            {
                Title         = dto.Title,
                Description   = dto.Description,
                ProjectUrl    = dto.ProjectUrl,
                GithubRepoUrl = dto.GithubRepoUrl,
                ImageUrl      = dto.ImageUrl,
                CreatedAt     = DateTime.UtcNow,
                ApplicantID   = applicantId
            };

            var added = await _repository.AddAsync(project);
            return MapToDto(added);
        }

        public async Task<bool> UpdateAsync(int projectId, int applicantId, ProjectDto dto)
        {
            var project = await _repository.GetByIdAsync(projectId, applicantId);
            if (project == null) return false;

            project.Title         = dto.Title;
            project.Description   = dto.Description;
            project.ProjectUrl    = dto.ProjectUrl;
            project.GithubRepoUrl = dto.GithubRepoUrl;
            project.ImageUrl      = dto.ImageUrl;
            project.UpdatedAt     = DateTime.UtcNow;

            await _repository.UpdateAsync(project);
            return true;
        }

        public async Task<bool> DeleteAsync(int projectId, int applicantId)
        {
            var project = await _repository.GetByIdAsync(projectId, applicantId);
            if (project == null) return false;

            await _repository.DeleteAsync(project);
            return true;
        }

        private static ProjectResponseDto MapToDto(Project p) => new ProjectResponseDto
        {
            ProjectID     = p.ProjectID,
            Title         = p.Title,
            Description   = p.Description,
            ProjectUrl    = p.ProjectUrl,
            GithubRepoUrl = p.GithubRepoUrl,
            ImageUrl      = p.ImageUrl,
            CreatedAt     = p.CreatedAt
        };
    }
}
