using System.Security.Claims;
using Graduation_Project.Dtos;
using Graduation_Project.Dtos.Company.Interview;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Company)]
    public class CompanyInterviewsController : ControllerBase
    {
        private readonly ICompanyInterviewService _interviewService;
        private readonly IApplicationServivces applicationServivces;

        public CompanyInterviewsController(ICompanyInterviewService interviewService,IApplicationServivces applicationServivces)
        {
            _interviewService = interviewService;
            this.applicationServivces = applicationServivces;
        }

 

        [HttpGet]
        public async Task<IActionResult> GetInterviews([FromQuery] string? search, [FromQuery] string? status)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetInterviewsAsync(companyId,search,status);
            var stats = await _interviewService.GetStatisticsAsync(companyId);
            return Ok(new {stats=stats,interviews=interviews});
        }

        [HttpGet("{interviewId}")]
        public async Task<IActionResult> GetInterviewDetails(Guid interviewId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var interview = await _interviewService.GetInterviewDetailsAsync(interviewId,companyId);
            if(interview == null)
                return NotFound("Interview not found");

            return Ok(interview);
        }

        [HttpPut("{interviewId}")]
        public async Task<IActionResult> UpdateInterview(Guid interviewId,[FromBody] UpdateCompanyInterviewDto dto)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _interviewService.UpdateInterviewAsync(interviewId,companyId,dto);
            if(!success)
                return NotFound("you don't have access");

            return Ok(new { message = "Interview updated successfully" });
        }
        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleInterview([FromBody] ScheduleInterviewDto dto)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");
            var id = await _interviewService.ScheduleInterviewAsync(dto);
            var result = await applicationServivces.ChangeApplicationStatus(dto.ApplicationId,companyId, ApplicationStatus.Accepted);
            if (!result)
            {
                return BadRequest(new { message = "error while change status" });

            }

            return Ok(new
            {
                message = "Interview scheduled successfully",
                interviewId = id
            });
        }

    }
}
