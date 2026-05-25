using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService subscriptionService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
        }
        [HttpPost("create")]
        //[Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> AddSubscriptionPlan(AddSubscriptionPlanDto subscriptionPlan)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await subscriptionService.AddAsync(subscriptionPlan);
            return Created();
        }
        [HttpPut("update/{Id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateSubscription([FromRoute]Guid Id,[FromBody]UpdateSubscriptionPlanDto updateSubscriptionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await subscriptionService.UpdateAsync(Id, updateSubscriptionDto);
            return Ok(new { Message = "Updated Success" });
        }
        [HttpDelete("delete/{Id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteSubcription([FromRoute] Guid Id)
        {
            await subscriptionService.DeleteAsync(Id);
            return Ok(new { Message = "Deleted Success" });

        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
           var result= await subscriptionService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await subscriptionService.GetByIdAsync(id);
            return Ok(result);
        }

    }
}
