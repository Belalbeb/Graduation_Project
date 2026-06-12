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
        #region Belal
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
        #endregion

        #region Applicants
        // ==================== USER MANAGEMENT ENDPOINTS ====================

        [HttpGet("users/stats")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserStats()
        {
            var stats = await adminServices.GetUserStatsAsync();
            return Ok(stats);
        }

        [HttpGet("users")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllApplicants()
        {
            var applicants = await adminServices.GetAllApplicantsAsync();
            return Ok(applicants);
        }

        [HttpGet("users/{applicantId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetApplicantDetails(Guid applicantId)
        {
            var details = await adminServices.GetApplicantDetailsAsync(applicantId);
            if(details == null)
                return NotFound(new { message = "Applicant not found" });
            return Ok(details);
        }

        [HttpPut("users/{applicantId}/block")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> BlockUser(Guid applicantId)
        {
            var result = await adminServices.BlockApplicantAsync(applicantId);
            if(!result)
                return NotFound(new { message = "Applicant not found" });
            return Ok(new { message = "User blocked successfully" });
        }

        [HttpPut("users/{applicantId}/unblock")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UnblockUser(Guid applicantId)
        {
            var result = await adminServices.UnblockApplicantAsync(applicantId);
            if(!result)
                return NotFound(new { message = "Applicant not found" });
            return Ok(new { message = "User unblocked successfully" });
        }

        [HttpPut("users/{applicantId}/approve")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ApproveUser(Guid applicantId)
        {
            var result = await adminServices.ApproveApplicantAsync(applicantId);
            if(!result)
                return NotFound(new { message = "Applicant not found" });
            return Ok(new { message = "User approved successfully" });
        }

        [HttpDelete("users/{applicantId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteUser(Guid applicantId)
        {
            var result = await adminServices.DeleteApplicantAsync(applicantId);
            if(!result)
                return NotFound(new { message = "Applicant not found" });
            return Ok(new { message = "User deleted successfully" });
        }
        #endregion

        #region Company
        // ==================== COMPANY MANAGEMENT ENDPOINTS ====================

        [HttpGet("companies/stats")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetCompanyStats()
        {
            var stats = await adminServices.GetCompanyStatsAsync();
            return Ok(stats);
        }

        [HttpGet("companies")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await adminServices.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("companies/{companyId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetCompanyDetails(Guid companyId)
        {
            var details = await adminServices.GetCompanyDetailsAsync(companyId);
            if(details == null)
                return NotFound(new { message = "Company not found" });
            return Ok(details);
        }

        [HttpPut("companies/{companyId}/verify")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> VerifyCompany(Guid companyId)
        {
            var result = await adminServices.VerifyCompanyAsync(companyId);
            if(!result)
                return NotFound(new { message = "Company not found" });
            return Ok(new { message = "Company verified successfully" });
        }

        [HttpPut("companies/{companyId}/block")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> BlockCompany(Guid companyId)
        {
            var result = await adminServices.BlockCompanyAsync(companyId);
            if(!result)
                return NotFound(new { message = "Company not found" });
            return Ok(new { message = "Company blocked successfully" });
        }

        [HttpPut("companies/{companyId}/unblock")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UnblockCompany(Guid companyId)
        {
            var result = await adminServices.UnblockCompanyAsync(companyId);
            if(!result)
                return NotFound(new { message = "Company not found" });
            return Ok(new { message = "Company unblocked successfully" });
        }

        [HttpDelete("companies/{companyId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var result = await adminServices.DeleteCompanyAsync(companyId);
            if(!result)
                return NotFound(new { message = "Company not found" });
            return Ok(new { message = "Company deleted successfully" });
        }
        #endregion
    }
}
