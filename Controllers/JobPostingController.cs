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

        public JobPostingController(IJobPostingService service)
        {
            _service = service;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _service.GetAllJobsAsync();
            return Ok(jobs);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _service.GetJobByIdAsync(id);
            if (job == null) return NotFound("Job not found");

            return Ok(job);
        }

        
        [HttpGet("company")]
        [Authorize(Roles=Roles.Company)]
        public async Task<IActionResult> GetByCompany()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!int.TryParse(profileIdClaim, out int companyId))
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
            if (!int.TryParse(profileIdClaim, out int companyId))
                return Unauthorized("Invalid or missing ProfileId");

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
            return Created();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobPosting job)
        {
            var updated = await _service.UpdateJobAsync(id, job);
            if (updated == null) return NotFound("Job not found");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteJobAsync(id);
            if (!deleted) return NotFound("Job not found");

            return Ok("Job deleted");
        }

        
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var success = await _service.DeactivateJobAsync(id);
            if (!success) return NotFound("Job not found");

            return Ok("Job deactivated");
        }

    }
}
