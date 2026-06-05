using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ApplicationController(IApplicationServivces applicationServivces,IApplicantServices applicantServices)
        {
            this.applicationServivces = applicationServivces;
            this.applicantServices = applicantServices;
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
        [HttpPut("change-application-Status/{ApplicationId}")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> ChangeApplicationStatus(
    Guid ApplicationId,
    ChangeStatusDto dto)
        {
            var result = await applicationServivces
                .ChangeApplicationStatus(ApplicationId, dto.Status);

            if (!result)
                return BadRequest();

            return Ok();
        }
        [HttpPost]
        [Authorize(Roles =Roles.Applicant)]
        public async Task<IActionResult> CreateApplication(CreateApplicationDto createApplicationDto)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileIdClaim, out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");
          var result=  await applicationServivces.CreateApplication(applicantId, createApplicationDto);
            return Ok(new {message="created success", applicationId=result.ApplicationID});

        }
    }
}
