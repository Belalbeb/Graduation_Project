using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly CloudinaryService cloudinaryService;

        public ProjectService(IProjectRepository repository,CloudinaryService cloudinaryService)
        {
            _repository = repository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<List<ProjectResponseDto>> GetAllAsync(Guid applicantId)
        {
            var projects = await _repository.GetAllByApplicantAsync(applicantId);
            return projects.Select(MapToDto).ToList();
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(Guid projectId, Guid applicantId)
        {
            var project = await _repository.GetByIdAsync(projectId, applicantId);
            return project == null ? null : MapToDto(project);
        }

        public async Task<ProjectResponseDto?> AddAsync(Guid applicantId, CreateProjectDto dto)
        {
            var project = new Project
            {
                Title         = dto.Title,
                Description   = dto.Description,
                ProjectUrl    = dto.ProjectUrl,
                GithubRepoUrl = dto.GithubRepoUrl,
                ImageUrl      = await cloudinaryService.UploadImageAsync(dto.Image),
                CreatedAt     = DateTime.UtcNow,
                ApplicantID   = applicantId
            };

            var added = await _repository.AddAsync(project);
            return MapToDto(added);
        }

        public async Task<bool> UpdateAsync(Guid projectId, Guid applicantId, ProjectDto dto)
        {
            var project = await _repository.GetByIdAsync(projectId, applicantId);

            if (project == null)
                return false;

            
            if (!string.IsNullOrWhiteSpace(dto.Title))
                project.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                project.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.ProjectUrl))
                project.ProjectUrl = dto.ProjectUrl;

            if (!string.IsNullOrWhiteSpace(dto.GithubRepoUrl))
                project.GithubRepoUrl = dto.GithubRepoUrl;

            if (dto.Image != null)
            {
                var imageUrl = await cloudinaryService.UploadImageAsync(dto.Image);

                if (!string.IsNullOrEmpty(imageUrl))
                    project.ImageUrl = imageUrl;
            }

            project.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(project);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid projectId, Guid applicantId)
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
