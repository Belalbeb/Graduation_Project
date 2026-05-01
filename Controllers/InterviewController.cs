using System.Security.Claims;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewServices _interviewServices ;

        public InterviewController(IInterviewServices interviewServices)
        {
            _interviewServices = interviewServices ;
        }

        [HttpGet("UpcomingInterviews")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetUpcomingInterviews()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim, out int applicantId))
                return Unauthorized("Invalid or missing ProfileId") ;

            var result = await _interviewServices.GetUpcomingInterviewsAsync(applicantId) ;
            if (result == null) return BadRequest() ;
            return Ok(result) ;
        }

        [HttpGet("CompletedInterviews")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetCompletedInterviews()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var result = await _interviewServices.GetCompletedInterviewsAsync(applicantId);
            if(result == null) return BadRequest();
            return Ok(result);
        }

        [HttpGet("CancelledInterviews")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetCancelledInterviews()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var result = await _interviewServices.GetCancelledInterviewsAsync(applicantId);
            if(result == null) return BadRequest();
            return Ok(result);
        }
    }
}
