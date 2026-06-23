using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _service;

        public CouponController(ICouponService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(CreateCouponDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAll([FromQuery] QueryCouponDto query)
        {
            var result = await _service.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("code/{code}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetByCode(string code)
        {
            var result = await _service.GetByCodeAsync(code);
            return Ok(result);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCouponDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(new {message="deleted success"});
        }
        [Authorize(Roles =Roles.Company)]
        [HttpPost("validate")]
        public async Task<IActionResult> Validate(ValidateCouponDto dto)
        {
            var result = await _service.ValidateAsync(dto);
            return Ok(result);
        }

        [HttpPost("apply")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Apply(ApplyCouponDto dto)
        {
            await _service.ApplyAsync(dto);
            return Ok(new { message = "Applied success" });
        }

        [HttpPost("{id}/revoke")]
        public async Task<IActionResult> Revoke(Guid id)
        {
            await _service.RevokeAsync(id);
            return Ok(new { message = "revoked success" });
        }

      
    }
}