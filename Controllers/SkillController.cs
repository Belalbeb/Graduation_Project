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
    public class SkillController : ControllerBase
    {
        private readonly IApplicantSkillService _skillService ;

        public SkillController(IApplicantSkillService skillService)
        {
            _skillService = skillService ;
        }

        [HttpGet]
        public async Task<IActionResult> GetMySkills()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var skills = await _skillService.GetAllSkillsAsync(applicantId) ;
            return Ok(skills) ;
        }

        [HttpGet("{skillId}")]
        public async Task<IActionResult> GetSkillById(int skillId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var skill = await _skillService.GetSkillByIdAsync(skillId, applicantId) ;

            if(skill == null)
                return NotFound($"Skill with ID {skillId} not found or you don't have access.") ;

            return Ok(skill) ;
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill([FromBody] SkillDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState) ;

            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var result = await _skillService.AddSkillAsync(applicantId, dto) ;

            if(result == null)
                return BadRequest("This skill has already been added to your profile.") ;

            return CreatedAtAction(nameof(GetSkillById),
                new { skillId = result.ApplicantSkillID }, result);
        }

        [HttpDelete("{applicantSkillId}")]
        public async Task<IActionResult> DeleteSkill(int applicantSkillId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _skillService.DeleteSkillAsync(applicantSkillId, applicantId) ;

            if(!success)
                return NotFound($"Skill with ID {applicantSkillId} not found") ;

            return NoContent();
        }
    }
}
