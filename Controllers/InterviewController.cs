using System.Security.Claims;
using Graduation_Project.Dtos;
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
        private readonly IInterviewServices _interviewService ;

        public InterviewController(IInterviewServices interviewService)
        {
            _interviewService = interviewService ;
        }

        [HttpGet("statistics")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetStatistics()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var stats = await _interviewService.GetStatisticsAsync(applicantId);
            return Ok(stats);
        }

        [HttpGet("upcoming")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetUpcoming()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetUpcomingAsync(applicantId);
            return Ok(interviews);
        }

        [HttpGet("completed")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetCompleted()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetCompletedAsync(applicantId);
            return Ok(interviews);
        }

        [HttpGet("cancelled")]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetCancelled()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetCancelledAsync(applicantId);
            return Ok(interviews);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Applicant)]
        public async Task<IActionResult> GetAll()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");
            var InterviewStatisticsDto = await _interviewService.GetStatisticsAsync(applicantId);
            var interviews = await _interviewService.GetAllAsync(applicantId);
            return Ok(new {stats= InterviewStatisticsDto, interviews =interviews });
        }
        [HttpGet("get-by-company/{id}")]
        [Authorize(Roles =Roles.Company)]
        public async Task<IActionResult> GetInterviewById(Guid id)
        {
            var result = await _interviewService.InterviewCompanyDetails(id);
            if (result == null) return NotFound(new { message = "No Interview Found" });
            return Ok(result);
        }
        [HttpPut("change-interview-date/{InterviewId}")]
        [Authorize(Roles=Roles.Company)]
        public async Task<IActionResult> ChangeInterviewDate([FromRoute]Guid InterviewId,ChangeInterviewDateDto interviewDateDto)
        {
         var result= await _interviewService.ChangeInterviewDate(InterviewId, interviewDateDto.InterviewDate);
            if (!result) return BadRequest(new { message = "no interview found" });
            return Ok();
        }
        
    }
}
