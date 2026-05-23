using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices adminServices;

        public AdminController(IAdminServices adminServices)
        {
            this.adminServices = adminServices;
        }
        [HttpGet("dashboard-overview")]
        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> GetAdminDashboard()
        {

            var dashboard =await adminServices.GetAdminDashboardAsync();
            if (dashboard == null) return NotFound(new { message = "dashboard not found" });
            return Ok(dashboard);



        }
        [HttpGet("jobs-overview")]
        [Authorize(Roles=Roles.Admin)]
        public async Task<IActionResult> GetAdminJobDashboard()
        {
            var jobDashboard = await adminServices.GetAdminJobDashboard();
            if (jobDashboard == null) return NotFound(new { message = "dashboard not found" });
            return Ok(jobDashboard);
        }
        [HttpGet("job-details/{jobId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAdminJobDetails([FromRoute]Guid jobId)
        {
            var job = await adminServices.AdminJobDetails(jobId);
            if (job == null) return NotFound(new { Message = "no job found with this Id" });

            return Ok(job);


        }
        [HttpPut("accept-job/{jobId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AcceptJob([FromRoute] Guid jobId)
        {
            var result = await adminServices.AcceptJobAsync(jobId);
            if (!result)
            {
                return BadRequest(new { message = "there is error please try again later" });

            }
            return Ok(new { message = "Accepted" });


        }
        [HttpPut("reject-job/{jobId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> rejectJob([FromRoute]Guid jobId)
        {
            var result = await adminServices.RejectJobAsync(jobId);
            if (!result)
            {
                return BadRequest(new { message = "there is error please try again later" });

            }
            return Ok(new { message = "Rejected" });


        }
    }
}
