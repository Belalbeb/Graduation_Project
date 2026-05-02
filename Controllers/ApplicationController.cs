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


        [HttpGet("GetMyApplications")]
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
                return NotFound("No applications found");

            return Ok(result);
        }
    }
}
