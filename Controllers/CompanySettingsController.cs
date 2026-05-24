using Graduation_Project.Models;
using System.Security.Claims;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project.Dtos.Company.SettingsTabs;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Company)]
    public class CompanySettingsController : ControllerBase
    {
        private readonly ICompanySettingsService _settingsService ;

        public CompanySettingsController(ICompanySettingsService settingsService)
        {
            _settingsService = settingsService ;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var profile = await _settingsService.GetProfileDetailsAsync(companyId);

            if(profile == null)
                return NotFound("Company profile not found.");

            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateCompanyProfileDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _settingsService.UpdateProfileAsync(companyId,dto);

            if(!success)
                return BadRequest("Failed to update company profile.");

            return Ok(new { message = "Company profile updated successfully" });
        }

        // ====================== Socials Tab ======================

        [HttpGet("socials")]
        public async Task<IActionResult> GetSocials()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var socials = await _settingsService.GetSocialsDetailsAsync(companyId);

            if(socials == null)
                return NotFound("Company profile not found.");

            return Ok(socials);
        }

        [HttpPut("socials")]
        public async Task<IActionResult> UpdateSocials([FromBody] UpdateCompanySocialsDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _settingsService.UpdateSocialsAsync(companyId,dto);

            if(!success)
                return BadRequest("Failed to update social information.");

            return Ok(new { message = "Social information updated successfully" });
        }
    }
}
