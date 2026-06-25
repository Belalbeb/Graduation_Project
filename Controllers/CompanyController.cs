using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyServices companyServices;
        private readonly ICompanyVerificationService companyVerificationService;
        private readonly ISubscriptionService subscriptionService;

        public CompanyController(ICompanyServices companyServices,ICompanyVerificationService companyVerificationService,ISubscriptionService subscriptionService)
        {
            this.companyServices = companyServices;
            this.companyVerificationService = companyVerificationService;
            this.subscriptionService = subscriptionService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(Guid id)
        {
            var company = await companyServices.GetCompanyByIdAsync(id);
            if (company == null) return NotFound();
            return Ok(company);
        }
        [HttpGet("get-public-profile")]
        public async Task<IActionResult> GetPublicProfile(Guid companyId)
        {
            var profile = await companyServices.GetCompanyProfileAsync(companyId);

            if (profile == null)
                return NotFound("Company profile not found");

            return Ok(profile);

        }


        [HttpGet("get-dashboard")]
        [Authorize(Roles=Roles.Company)]
        public async Task<IActionResult> GetCompanyDashboard()
        {
            //var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            //if (!int.TryParse(profileIdClaim, out int companyId))
            //    return Unauthorized("Invalid or missing ProfileId");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await companyServices.GetCompanyByUserIdAsync(userId);

            if (company == null)
                return NotFound();
            var companyDashboard = await companyServices.GetCompanyDashboardAsync(company.CompanyID);
            if (companyDashboard == null) return NotFound();
            return Ok(companyDashboard);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] Company company)
        {
            if (!ModelState.IsValid) return BadRequest();

            var createdCompany = await companyServices.AddCompanyAsync(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.CompanyID }, createdCompany);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] Company updatedCompany)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await companyServices.UpdateCompanyAsync(id, updatedCompany);
            if (!result) return NotFound();

            return NoContent(); 
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var result = await companyServices.DeleteCompanyAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpGet("my-profile")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> GetMyProfile()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if(!Guid.TryParse(profileIdClaim,out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            var profile = await companyServices.GetCompanyProfileAsync(companyId);

            if(profile == null)
                return NotFound("Company profile not found");

            return Ok(profile);
        }
        [HttpPost("verification-request")]
        public async Task<IActionResult> CreateRequest([FromForm] CreateVerificationRequestDto dto)
        {
            var companyIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(companyIdClaim, out Guid companyId))
                return Unauthorized();

            var result = await companyVerificationService.CreateRequestAsync(companyId, dto.Documents);

            if (!result)
                return BadRequest("Failed to create request");

            return Ok("Verification request submitted");
        }
        [HttpGet("candidates")]
        [Authorize(Roles=Roles.Company)]
        public async Task<IActionResult> GetCandidatesForCompany([FromQuery] int page = 1, [FromQuery] CandidateFilterDto filter = null)
        {
            var companyIdClaim = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(companyIdClaim, out Guid companyId))
                return Unauthorized();
            bool HasCandidateSearch = await subscriptionService.HasCandidateSearch(companyId);
            if (!HasCandidateSearch)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = "Upgrade your subscription to see relevant candidates."
                });
            }


            if (page < 1)
                return BadRequest(new
                {
                    Message = "Page must be greater than or equal to 1."
                });

            var candidates = await companyServices.GetAllCandidate(page, filter);

            return Ok(candidates);
        }
        [HttpGet("can-post-job")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> CanPostJobs()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            bool hasReachTheMaxJobPosting = await subscriptionService.HasReachTheMaxJobPosting(companyId);
            if (hasReachTheMaxJobPosting)
            {
                return Ok(new { canPost = false });
            }

            return Ok(new { canPost = true });

        }
        [HttpGet("has-ai-access")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> HasAiToolAccess()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            bool result = await subscriptionService.HasAiToolAccess(companyId);
            if (!result)
            {
                return Ok(new { hasAiToolAccess = false });
            }

            return Ok(new { hasAiToolAccess = true });

        }
        [HttpGet("has-candidate-search")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> HasCandidatesSearch()
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            bool result = await subscriptionService.HasCandidateSearch(companyId);
            if (!result)
            {
                return Ok(new { hasCandidatesSearch = false });
            }

            return Ok(new { hasCandidatesSearch = true });

        }
    }
}
