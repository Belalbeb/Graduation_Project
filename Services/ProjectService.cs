using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context ;
        public ProjectService(ApplicationDbContext context)
        {
            _context = context ;
        }

        public async Task<List<ProjectResponseDto>> GetAllAsync(int applicantId)
        {
            return await _context.Projects
                .Where(p => p.ApplicantID == applicantId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProjectResponseDto
                {
                    ProjectID = p.ProjectID,
                    Title = p.Title,
                    Description = p.Description,
                    ProjectUrl = p.ProjectUrl,
                    GithubRepoUrl = p.GithubRepoUrl,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(int projectId,int applicantId)
        {
            return await _context.Projects
                .Where(p => p.ProjectID == projectId && p.ApplicantID == applicantId)
                .Select(p => new ProjectResponseDto
                {
                    ProjectID = p.ProjectID,
                    Title = p.Title,
                    Description = p.Description,
                    ProjectUrl = p.ProjectUrl,
                    GithubRepoUrl = p.GithubRepoUrl,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ProjectResponseDto?> AddAsync(int applicantId, ProjectDto dto)
        {
            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                ProjectUrl = dto.ProjectUrl,
                GithubRepoUrl = dto.GithubRepoUrl,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                ApplicantID = applicantId
            } ;

            await _context.Projects.AddAsync(project) ;
            await _context.SaveChangesAsync() ;

            return new ProjectResponseDto
            {
                ProjectID = project.ProjectID,
                Title = project.Title,
                Description = project.Description,
                ProjectUrl = project.ProjectUrl,
                GithubRepoUrl = project.GithubRepoUrl,
                ImageUrl = project.ImageUrl,
                CreatedAt = project.CreatedAt
            } ;
        }

        public async Task<bool> UpdateAsync(int projectId, int applicantId, ProjectDto dto)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectID == projectId && p.ApplicantID == applicantId);

            if(project == null)
                return false;

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.ProjectUrl = dto.ProjectUrl;
            project.GithubRepoUrl = dto.GithubRepoUrl;
            project.ImageUrl = dto.ImageUrl;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int projectId,int applicantId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectID == projectId && p.ApplicantID == applicantId);

            if(project == null)
                return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
