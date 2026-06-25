using Graduation_Project.Dtos;
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
    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingService _service;
        private readonly ISubscriptionService subscriptionService;

        public JobPostingController(IJobPostingService service,ISubscriptionService subscriptionService)
        {
            _service = service;
            this.subscriptionService = subscriptionService;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]JobFilterDto jobFilterDto)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            Guid.TryParse(profileIdClaim, out Guid ApplicantId);
               
            var jobs = await _service.GetAllJobsAsync(ApplicantId,jobFilterDto);
            return Ok(jobs);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            Guid.TryParse(profileIdClaim, out Guid ApplicantId);

            var job = await _service.GetJobByIdAsync(id,ApplicantId);
            var similarJobs = await _service.GetSimilarJobsAsync(id, ApplicantId);
            if (job == null) return NotFound("Job not found");

            return Ok(new {job=job,similarJobs=similarJobs});
        }

        
        [HttpGet("company")]
        [Authorize(Roles=Roles.Company)]
        public async Task<IActionResult> GetByCompany()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");
            var jobs = await _service.GetJobsByCompanyAsync(companyId);
            if (jobs == null) return NotFound("no jobs founded for this company");
            return Ok(jobs);
        }

        
        [HttpPost]
        [Authorize(Roles =Roles.Company)]
        public async Task<IActionResult> Create([FromBody] CreateJobDto job)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");
            bool hasReachTheMaxJobPosting = await subscriptionService.HasReachTheMaxJobPosting(companyId);
            if (hasReachTheMaxJobPosting)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = "You have reached the maximum number of job postings for this month. Please upgrade your plan."
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage)
                    });

                return BadRequest(errors);
            }

            var newJob = await _service.CreateJobAsync(job, companyId);
            return Created(string.Empty, newJob);
        }
        [HttpGet("job-details/{jobId}")]
        public async Task<IActionResult> GetJobDetails(Guid jobId)
        {
            if (jobId == Guid.Empty) return BadRequest(new { message = "jobId is Required" });
            var jobs =await _service.JobDetails(jobId);
            if (jobs == null) return NotFound(new { message = "no job found for this company" });
            return Ok(jobs);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateJobDto job)
        {
            var updated = await _service.UpdateJobAsync(id, job);
            if (!updated) return NotFound("Job not found");

            return Ok(new { message ="Updated Succefully"});

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var deleted = await _service.DeleteJobAsync(id);
            if (!deleted) return NotFound("Job not found");

            return Ok(new {message= "Job deleted" });
        }

        
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var success = await _service.DeactivateJobAsync(id);
            if (!success) return NotFound("Job not found");

            return Ok("Job deactivated");
        }

    }
}
