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
            var socials = await _settingsService.GetSocialsDetailsAsync(companyId);

            if (profile == null)
                return NotFound("Company profile not found.");

            return Ok(new {profile=profile,socials=socials});
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
        [HttpPut("cover-image")]
        public async Task<IActionResult> UpdateCoverImage([FromForm]IFormFile cover)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            if (cover.Length == 0) return BadRequest(new { message= "no file uploaded" });
            var result = await _settingsService.UpdateCoverImage(companyId, cover);
            if (!result) return BadRequest(new { message = "failed to upload to cloudinary please try again later" });
            return Ok(new { message = "updated success" });
        }
        [HttpPut("logo")]
        public async Task<IActionResult> UpdateLogoUrl([FromForm]IFormFile logo)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            if (logo.Length == 0) return BadRequest(new { message = "no file uploaded" });
            var result = await _settingsService.UpdateLogo(companyId, logo);
            if (!result) return BadRequest(new { message = "failed to upload to cloudinary please try again later" });
            return Ok(new { message = "updated success" });
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
