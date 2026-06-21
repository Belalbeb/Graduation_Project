using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices adminServices;
        private readonly ICompanyVerificationService companyVerificationService;
        private readonly IWebsiteSettingsService websiteSettingsService;

        public AdminController(IAdminServices adminServices,ICompanyVerificationService companyVerificationService,IWebsiteSettingsService websiteSettingsService)
        {
            this.adminServices = adminServices;
            this.companyVerificationService = companyVerificationService;
            this.websiteSettingsService = websiteSettingsService;
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


        [HttpGet("users")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllApplicants()
        {
            var applicants = await adminServices.GetAllApplicantsAsync();
            var stats = await adminServices.GetUserStatsAsync();

            return Ok(new {stats=stats,applicants=applicants});
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


        [HttpDelete("user/{applicantId}")]
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



        [HttpGet("companies")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await adminServices.GetAllCompaniesAsync();
            var stats = await adminServices.GetCompanyStatsAsync();
            return Ok(new {stats=stats,companies=companies});
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



        [HttpGet("verification-requests")]
        public async Task<IActionResult> GetPending()
        {
            var result = await companyVerificationService.GetAllRequestsAsync();

            

            return Ok(result);
        }
        [HttpGet("vertification-request-details/{id}")]
        public async Task<IActionResult> GetVertificationRequestDetails(Guid id)
        {
            var result = await companyVerificationService.GetVertificationRequestDetails(id);
            if (result == null) return NotFound(new { message = "no request founf with this Id" });
            return Ok(result);
        }


        [HttpPut("verification-requests/{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var result = await companyVerificationService.ApproveAsync(id);

            if (!result)
                return NotFound();

            return Ok("Company verified");
        }

        [HttpPut("verification-requests/{id}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            var result = await companyVerificationService.RejectAsync(id);

            if (!result)
                return NotFound();

            return Ok("Request rejected");
        }
        [HttpPut("verification-requests/{id}/request-more-information")]
        public async Task<IActionResult> RequestMoreInformation(Guid id,[FromBody] RequestMoreInformationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await companyVerificationService
                .RequestMoreInformationAsync(id, dto.Notes);

            if (!result)
                return NotFound();

            return Ok("Request updated. Company has been asked to provide more information.");
        }
        #endregion
        #region WebsiteSetting


        [HttpGet("website-settings")]
        public async Task<IActionResult> Get()
        {
            var result = await websiteSettingsService.GetAsync();

         

            return Ok(result);
        }
        [HttpPut("website-settings")]
        public async Task<IActionResult> Update([FromBody] UpdateWebsiteSettingsDto dto)
        {
            var result = await websiteSettingsService.UpdateAsync(dto);

            if (!result)
                return NotFound();

            return Ok("Settings updated successfully");
        }
        #endregion
    }
}

