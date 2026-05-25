using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =Roles.Admin)]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _service;

        public CouponController(ICouponService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCouponDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = result.Id },
                ApiResponse<CouponResponseDto>.Ok(result, "Coupon created"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryCouponDto query)
        {
            var result = await _service.GetAllAsync(query);
            return Ok(ApiResponse<PaginatedResult<CouponResponseDto>>.Ok(result));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return Ok(ApiResponse<CouponResponseDto>.Ok(result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<CouponResponseDto>.Fail(ex.Message));
            }
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                var result = await _service.GetByCodeAsync(code);
                return Ok(ApiResponse<CouponResponseDto>.Ok(result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<CouponResponseDto>.Fail(ex.Message));
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCouponDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);
                return Ok(ApiResponse<CouponResponseDto>.Ok(result, "Updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CouponResponseDto>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] ValidateCouponDto dto)
        {
            try
            {
                var result = await _service.ValidateAsync(dto);
                return Ok(ApiResponse<CouponValidationResult>.Ok(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CouponValidationResult>.Fail(ex.Message));
            }
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] ApplyCouponDto dto)
        {
            try
            {
                await _service.ApplyAsync(dto);
                return Ok(ApiResponse<object>.Ok(null, "Applied successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost("{id:guid}/revoke")]
        public async Task<IActionResult> Revoke(Guid id)
        {
            await _service.RevokeAsync(id);
            return Ok(ApiResponse<object>.Ok(null, "Revoked"));
        }

        [HttpGet("generate-code")]
        public async Task<IActionResult> GenerateCode([FromQuery] string? prefix)
        {
            var result = await _service.GenerateCodeAsync(prefix);
            return Ok(ApiResponse<GeneratedCodeDto>.Ok(result));
        }
    }
}