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
   
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService ;
        private readonly ICompanyServices companyServices;

        public ProfileController(IProfileService profileService,ICompanyServices companyServices)
        {
            _profileService = profileService ;
            this.companyServices = companyServices;
        }
        [Authorize]
        [HttpGet("{applicantId}")]
        public async Task<IActionResult> GetProfile(Guid applicantId)
        {
            //var currentUserApplicantId = GetCurrentApplicantIdFromClaims();

            // Optional: You can add extra logic here later (e.g., private profiles, visibility settings)

            var profile = await _profileService.GetPublicProfileAsync(applicantId);

            if(profile == null)
                return NotFound("Profile not found.");

            return Ok(profile);
        }
        [HttpGet("public-profile/{id}")]
        public async Task<IActionResult> GetPublicProfile([FromRoute]Guid id)
        {
            object? profile = await _profileService.GetPublicProfileAsync(id);

            if (profile == null)
            {
                profile = await companyServices.GetCompanyProfileAsync(id);
            }

            if (profile == null)
            {
                return NotFound("Profile not found.");
            }

            return Ok(profile);
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var profile = await _profileService.GetPublicProfileAsync(applicantId);

            if(profile == null)
                return NotFound("Profile not found.");

            return Ok(profile);
        }
    }
}
