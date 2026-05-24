using System.Security.Claims;
using Graduation_Project.Dtos.Company.Interview;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Company)]
    public class CompanyInterviewsController : ControllerBase
    {
        private readonly ICompanyInterviewService _interviewService;

        public CompanyInterviewsController(ICompanyInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var stats = await _interviewService.GetStatisticsAsync(companyId);
            return Ok(stats);
        }

        [HttpGet]
        public async Task<IActionResult> GetInterviews([FromQuery] string? search, [FromQuery] string? status)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var interviews = await _interviewService.GetInterviewsAsync(companyId,search,status);
            return Ok(interviews);
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
            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var success = await _interviewService.UpdateInterviewAsync(interviewId,companyId,dto);
            if(!success)
                return NotFound("Interview not found or you don't have access");

            return Ok(new { message = "Interview updated successfully" });
        }
    }
}
