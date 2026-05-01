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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyServices companyServices;

        public CompanyController(ICompanyServices companyServices)
        {
            this.companyServices = companyServices;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await companyServices.GetCompanyByIdAsync(id);
            if (company == null) return NotFound();
            return Ok(company);
        }


        [HttpGet("get-dashboard/{companyid}")]
        //[Authorize(Roles.Admin)]
        public async Task<IActionResult> GetCompanyByUserId(int companyId)
        {
            //var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            //if (!int.TryParse(profileIdClaim, out int companyId))
            //    return Unauthorized("Invalid or missing ProfileId");
            var company = await companyServices.GetCompanyDashboardAsync(companyId);
            if (company == null) return NotFound();
            return Ok(company);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] Company company)
        {
            if (!ModelState.IsValid) return BadRequest();

            var createdCompany = await companyServices.AddCompanyAsync(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.CompanyID }, createdCompany);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Company updatedCompany)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await companyServices.UpdateCompanyAsync(id, updatedCompany);
            if (!result) return NotFound();

            return NoContent(); 
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var result = await companyServices.DeleteCompanyAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
