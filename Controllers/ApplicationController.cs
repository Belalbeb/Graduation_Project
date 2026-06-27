using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationServivces applicationServivces;
        private readonly IApplicantServices applicantServices;
        private readonly IAiService aiService;

        public ApplicationController(IApplicationServivces applicationServivces,IApplicantServices applicantServices,IAiService aiService)
        {
            this.applicationServivces = applicationServivces;
            this.applicantServices = applicantServices;
            this.aiService = aiService;
        }


        [HttpGet("get-my-applications")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetApplicationByApplicant()
        {
            //var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            //if (!int.TryParse(profileIdClaim, out int applicantId))
            //    return Unauthorized("Invalid or missing ProfileId");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicant = await applicantServices.GetApplicantByUserIdAsync(userId);
            if (applicant == null) return NotFound();

            var result = await applicationServivces.GetApplicationByApplicant(applicant.ApplicantID);

            if (result == null || !result.Any())
                return NotFound(new {Message= "No applications found" });

            return Ok(result);
        }
        [HttpGet("{ApplicationId}")]
        public async Task<IActionResult> GetApplicantByApplicationID([FromRoute]Guid ApplicationId)
        {
            var result = await applicationServivces.GetApplicantByApplication(ApplicationId);
            if (result == null) return NotFound(new { message = "no application found" });
            return Ok(result);
        }
        [HttpPut("reviewed-application/{applicationId}")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> AcceptApplication(Guid applicationId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");
            var result = await applicationServivces
                .ChangeApplicationStatus(applicationId,companyId ,ApplicationStatus.Reviewed);

            if (!result)
                return BadRequest(new {message="sorry, you don't have access"});

            return Ok(new {message="updated success"});
        }
        [HttpPut("reject-application/{applicationId}")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> RejectApplication(Guid applicationId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");
            var result = await applicationServivces
                .ChangeApplicationStatus(applicationId, companyId,ApplicationStatus.Rejected);

            if (!result)
                return BadRequest(new { message = "sorry, you don't have access" });

            return Ok(new { message = "updated success" });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> CreateApplication(CreateApplicationDto createApplicationDto)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileIdClaim, out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var application = await applicationServivces.CreateApplication(
                applicantId,
                createApplicationDto);

            //try
            //{
            //    await aiService.CalculateMatchScoreAsync(application.ApplicationID);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            return Ok(new
            {
                message = "created success",
                applicationId = application.ApplicationID
            });
        }

    }
}
