using Graduation_Project.Models;
using System.Security.Claims;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Applicant)]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService ;
        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService ;
        }

        // Profile tab

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var profile = await _settingsService.GetProfileDetailsAsync(applicantId);

            if (profile == null)
                return NotFound("Profile not found.");

            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _settingsService.UpdateProfileAsync(applicantId, dto);

            if(!success)
                return BadRequest("Failed to update profile.");

            return Ok(new { message = "Profile updated successfully" });
        }

        // Contact tab

        [HttpGet("contact")]
        public async Task<IActionResult> GetContact()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var contact = await _settingsService.GetContactDetailsAsync(applicantId);
            return Ok(contact);
        }

        [HttpPut("contact")]
        public async Task<IActionResult> UpdateContact([FromBody] UpdateContactDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _settingsService.UpdateContactAsync(applicantId, dto);

            if(!success)
                return BadRequest("Failed to update contact information.");

            return Ok(new { message = "Contact information updated successfully" });
        }
    }
}
