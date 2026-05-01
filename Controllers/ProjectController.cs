using System.Security.Claims;
using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Applicant)]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService ;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService ;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProjects()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var projects = await _projectService.GetAllAsync(applicantId);
            return Ok(projects);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var project = await _projectService.GetByIdAsync(projectId,applicantId);

            if(project == null)
                return NotFound("Project not found.");

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] ProjectDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var result = await _projectService.AddAsync(applicantId, dto);

            if(result == null)
                return BadRequest("Failed to add project.");

            return CreatedAtAction(nameof(GetProjectById),
                new { projectId = result.ProjectID },result);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(int projectId,[FromBody] ProjectDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _projectService.UpdateAsync(projectId, applicantId, dto);

            if(!success)
                return NotFound("Project not found.");

            return Ok(new { message = "Project updated successfully" });
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _projectService.DeleteAsync(projectId, applicantId);

            if(!success)
                return NotFound("Project not found.");

            return NoContent();
        }
    }
}
