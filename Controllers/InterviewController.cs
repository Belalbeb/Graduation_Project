using System.Security.Claims;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Applicant)]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewServices _interviewService ;

        public InterviewController(IInterviewServices interviewService)
        {
            _interviewService = interviewService ;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var stats = await _interviewService.GetStatisticsAsync(applicantId);
            return Ok(stats);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetUpcomingAsync(applicantId);
            return Ok(interviews);
        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompleted()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetCompletedAsync(applicantId);
            return Ok(interviews);
        }

        [HttpGet("cancelled")]
        public async Task<IActionResult> GetCancelled()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetCancelledAsync(applicantId);
            return Ok(interviews);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!int.TryParse(profileIdClaim,out int applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetAllAsync(applicantId);
            return Ok(interviews);
        }
    }
}
