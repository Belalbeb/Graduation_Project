using Graduation_Project.Models;
using System.Security.Claims;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.Applicant)]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService ;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly IResumeServices resumeServices;

        public SettingsController(ISettingsService settingsService,UserManager<ApplicationUser> userManager,IEmailService emailService,IResumeServices resumeServices)
        {
            _settingsService = settingsService ;
            this.userManager = userManager;
            this.emailService = emailService;
            this.resumeServices = resumeServices;
        }

        // Profile tab

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
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

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _settingsService.UpdateProfileAsync(applicantId, dto);

            if(!success)
                return BadRequest(new {message="no thing to update or you dont have access" });

            return Ok(new { message = "Profile updated successfully" });
        }
        [HttpPut("update-photo")]
        public async Task<IActionResult> UpdatePhoto([FromForm] IFormFile photo)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileIdClaim, out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            if (photo == null || photo.Length == 0)
                return BadRequest("Photo is required");

            var result = await _settingsService.updatePhoto(applicantId, photo);

            if (!result)
                return BadRequest("Failed to update photo");

            return Ok("Photo updated successfully");
        }
        [HttpPut("update-cover-photo")]
        public async Task<IActionResult> UpdateCoverPhoto([FromForm] IFormFile coverPhoto)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileIdClaim, out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            if (coverPhoto == null || coverPhoto.Length == 0)
                return BadRequest("Cover photo is required");

            var result = await _settingsService.UpdateCoverPhoto(applicantId, coverPhoto);

            if (!result)
                return BadRequest("Failed to update cover photo");

            return Ok("Cover photo updated successfully");
        }
        [HttpPut("update-resume")]
        public async Task<IActionResult> UpdateResume([FromForm] UpdateResumeDto dto)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileIdClaim, out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            if (dto.Resume == null || dto.Resume.Length == 0)
                return BadRequest("Resume is required");
            if (string.IsNullOrWhiteSpace(dto.ResumeName))
                return BadRequest("Resume name is required");

            var result = await _settingsService.UpdateResume(applicantId, dto.Resume,dto.ResumeName);

            if (!result)
                return BadRequest("Failed to update resume");

            return Ok("Resume updated successfully");
        }
        
        [HttpDelete("delete-resume/{ResumeId}")]
        [Authorize(Roles=Roles.Applicant)]
        public async Task<IActionResult> DeleteResume([FromRoute]Guid ResumeId)
        {

          var result= await resumeServices.DeleteCV(ResumeId);
            if (!result) return BadRequest(new { message = "not exist this cv" });
            return Ok(new { message = "deleted success" });
            
        }
        // Contact tab

        [HttpGet("contact")]
        public async Task<IActionResult> GetContact()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
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

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _settingsService.UpdateContactAsync(applicantId, dto);

            if(!success)
                return BadRequest("Failed to update contact information.");

            return Ok(new { message = "Contact information updated successfully" });
        }
        [HttpPut("update-password")]
        [Authorize]
        public async Task<IActionResult>UpdatePasword(updatePassword dto)
        {
            var user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null) return NotFound("User Not Found");
            var result =await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);

            }
            return Ok(new { message = "password updated" });


        }
        [HttpPut("update-email")]
        [Authorize]
        public async Task<IActionResult> UpdateEmail(UpdateEmail dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            // generate token
            var token = await userManager.GenerateChangeEmailTokenAsync(
                user,
                dto.NewEmail);

            
            token = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            var frontendUrl = "http://localhost:3000";

            var confirmationLink =
                $"{frontendUrl}/confirm-email-change" +
                $"?userId={user.Id}" +
                $"&newEmail={Uri.EscapeDataString(dto.NewEmail)}" +
                $"&token={Uri.EscapeDataString(token)}";

            //var baseUrl = "http://jobify-api.runasp.net";

            //var confirmationLink =
            //    $"{baseUrl}/api/Settings/confirm-email-change" +
            //    $"?userId={user.Id}" +
            //    $"&newEmail={Uri.EscapeDataString(dto.NewEmail)}" +
            //    $"&token={Uri.EscapeDataString(token)}";

            // send email
            await emailService.SendEmailAsync(
                user.Email,
                "Confirm Email Change",
                confirmationLink);

            return Ok("Confirmation email sent to your current email.");
        }

        [HttpGet("confirm-email-change")]
        public async Task<IActionResult> ConfirmEmailChange(
            string userId,
            string newEmail,
            string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            token = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(token));

            var result = await userManager.ChangeEmailAsync(
                user,
                newEmail,
                token);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            user.UserName = newEmail;

            await userManager.UpdateAsync(user);
            await userManager.SetUserNameAsync(user, newEmail);


            return Ok("Email updated successfully.");
        }
    }
}
