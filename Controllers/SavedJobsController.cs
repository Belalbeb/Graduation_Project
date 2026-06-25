using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SavedJobsController : ControllerBase
    {
        private readonly ISavedJobService _service;

        public SavedJobsController(
            ISavedJobService service)
        {
            _service = service;
        }

        [HttpPost("{jobId}")]
       
        public async Task<IActionResult> SaveJob(Guid jobId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");
            await _service.SaveJobAsync(
                applicantId,
                jobId);

            return Ok(new
            {
                Message = "Job saved successfully"
            });
        }
        [HttpDelete("{jobId}")]
        public async Task<IActionResult> UnsaveJob(Guid jobId)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileIdClaim, out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            await _service.UnsaveJobAsync(applicantId, jobId);

            return Ok(new
            {
                Message = "Job unsaved successfully"
            });
        }
        [HttpGet]
      
        public async Task<IActionResult> GetSavedJobs()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid applicantId))
                return Unauthorized("Invalid or missing ProfileId");

            var result = await _service
                .GetSavedJobsAsync(applicantId);

            return Ok(result);
        }

    }
}
