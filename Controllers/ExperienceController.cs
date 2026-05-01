using Graduation_Project.Models;
using System.Security.Claims;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project.Dtos;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceService _experienceService ;
        public ExperienceController(IExperienceService experienceService)
        {
            _experienceService = experienceService ;
        }

        [HttpGet("{experienceId}")]
        public async Task<IActionResult> GetExperienceById([FromRoute] int experienceId)
        {
            var result = await _experienceService.GetExperienceByIdAsync(experienceId) ;
            if (result == null)
                return NotFound("Expereience Not Found") ;
            return Ok(result) ;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllApplicantExperiences()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var result = await _experienceService.GetAllAsync(applicantId) ;
            return Ok(result) ;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> AddExperience([FromBody] ExperienceDto experienceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest() ;
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var experience = new Experience
            {
                CompanyName = experienceDto.CompanyName,
                Location = experienceDto.Location,
                JobTitle = experienceDto.JobTitle,
                Description = experienceDto.Description,
                JobType = experienceDto.JobType,
                StartDate = experienceDto.StartDate,
                EndDate = experienceDto.EndDate,
                ApplicantID = applicantId
            } ;

            var result = await _experienceService.AddExperienceAsync(experience) ;
            if (result == null)
                return BadRequest("Failed to add experience. The date range overlaps with an existing experience.") ;

            return CreatedAtAction(nameof(GetExperienceById), new {experienceId = result.ExperienceID}, result) ;
        }
        // inject services: interview, Experience

        [HttpPut("{experienceId}")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> EditExperience([FromRoute] int experienceId, [FromBody] ExperienceDto experienceDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState) ;

            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var result = await _experienceService.UpdateExperienceAsync(experienceId, experienceDto) ;

            if (result == 1)
                return NotFound($"There is no experience with id {experienceId}") ;
            else if (result == 2)
                return BadRequest("Failed to update experience. The date range overlaps with an existing experience.") ;
            return NoContent() ;
        }

        [HttpDelete("{experienceId}")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> RemoveExperience([FromRoute] int experienceId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var result = await _experienceService.DeleteExperienceAsync(experienceId) ;
            return result ? NoContent() : NotFound() ;
        }
    }
}
