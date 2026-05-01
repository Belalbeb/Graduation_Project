using Graduation_Project.Models;
using System.Security.Claims;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService ;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService ;
        }

        [HttpGet("{applicantId}")]
        public async Task<IActionResult> GetProfile(int applicantId)
        {
            //var currentUserApplicantId = GetCurrentApplicantIdFromClaims();

            // Optional: You can add extra logic here later (e.g., private profiles, visibility settings)

            var profile = await _profileService.GetPublicProfileAsync(applicantId);

            if(profile == null)
                return NotFound("Profile not found.");

            return Ok(profile);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var profile = await _profileService.GetPublicProfileAsync(applicantId);

            if(profile == null)
                return NotFound("Profile not found.");

            return Ok(profile);
        }
    }
}
