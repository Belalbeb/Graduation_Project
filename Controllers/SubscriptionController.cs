using Graduation_Project.Dtos;
using Graduation_Project.Exceptions;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICouponService couponService;

        public SubscriptionController(ISubscriptionService subscriptionService,ApplicationDbContext applicationDbContext, ICouponService couponService)
        {
            SubscriptionService = subscriptionService;
            this._context = applicationDbContext;
            this.couponService = couponService;
        }

        public ISubscriptionService SubscriptionService { get; }

        [HttpPost("purchase")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> PurchaseSubscription(CreateSubscriptionDto dto)
        {
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            await SubscriptionService.SubscribeAsync(companyId, dto);

            return Ok("Subscription purchased successfully");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var result = await SubscriptionService.GetAllAsync();

            return Ok(result);
        }
        [HttpGet("dashboard-overview")]
        [Authorize(Roles=Roles.Admin)]
        public async Task<IActionResult> GetDashboardOverview()
        {
            var dashboard = await SubscriptionService.GetDashboardAsync();

     
            return Ok(dashboard);
        }

        [HttpGet("{subscriptionId}")]
        public async Task<IActionResult> GetSubscriptionDetails(
           Guid subscriptionId)
        {
            var result =
                await SubscriptionService.
                    GetFullDetailsBySubscriptionIdAsync(subscriptionId);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Subscription not found."
                });
            }

            return Ok(result);
        }
        [HttpGet("company-subscription")]

        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles=Roles.Company)]
        public async Task<IActionResult> GetSubscriptionPage()
        {
            
            var profileIdClaim = User.FindFirstValue(CustomClaims.ProfileId);
            if (!Guid.TryParse(profileIdClaim, out Guid companyId))
                return Unauthorized("Invalid or missing ProfileId");

            try
            {
                var result = await SubscriptionService.GetSubscriptionPageAsync(companyId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("checkout")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> CreateCheckout(CreateSubscriptionDto dto)
        {
            var profileId = User.FindFirstValue(CustomClaims.ProfileId);

            if (!Guid.TryParse(profileId, out Guid companyId))
                return Unauthorized("Invalid company");

            var plan = await _context.subscriptionPlans
                .FirstOrDefaultAsync(x => x.Id == dto.SubscriptionPlanId);

            if (plan == null)
                return NotFound("Plan not found");


            if (!Enum.TryParse<BillingCycle>(dto.BillingCycle, true, out var billingCycle))
            {
                throw new BadRequestException("Invalid billing cycle.");
            }

            var basePrice = billingCycle == BillingCycle.Yearly
                ? plan.YearlyPrice
                : plan.MonthlyPrice;

            decimal finalPrice = basePrice;

            //CouponResponseDto coupon = null;

          
            if (!string.IsNullOrEmpty(dto.CouponCode))
            {
                var validation = await couponService.ValidateAsync(
                    new ValidateCouponDto
                    {
                        Code = dto.CouponCode,
                        SubscriptionPlanId = dto.SubscriptionPlanId
                    });

                if (validation.IsValid)
                {
                    finalPrice = basePrice - (basePrice * validation.DiscountAmount / 100);
                    //coupon = validation.Coupon;
                }
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",

                SuccessUrl = "http://localhost:3000/subscription?isSuccess=true",
                CancelUrl = "http://localhost:3000/subscription?isSuccess=false",

                Metadata = new Dictionary<string, string>
              {
                 { "companyId", companyId.ToString() },
                 { "planId", plan.Id.ToString() },
                 { "billingCycle", dto.BillingCycle.ToString() },
                 { "couponCode", dto.CouponCode ?? "" }
    },

                LineItems = new List<SessionLineItemOptions>
              {
        new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = "usd",
                UnitAmount = (long)(finalPrice * 100),

                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = plan.Name
                }
            },
            Quantity = 1
        }
    }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(new
            {
                checkoutUrl = session.Url
            });
        }



        //stripe listen --forward-to https://localhost:7109/api/subscription/webhook
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body)
                .ReadToEndAsync();

            var stripeEvent = EventUtility.ParseEvent(json);

            if (stripeEvent.Type != "checkout.session.completed")
                return Ok();

            var session = stripeEvent.Data.Object as Session;

            if (session == null)
                return BadRequest("Invalid session");

      
            var companyId = Guid.Parse(session.Metadata["companyId"]);
            var planId = Guid.Parse(session.Metadata["planId"]);
            var billingCycle = session.Metadata["billingCycle"];
            var couponCode = session.Metadata.ContainsKey("couponCode")
                ? session.Metadata["couponCode"]
                : null;

            var amount = session.AmountTotal ?? 0;

            var current =
          await SubscriptionService
             .GetActiveSubscriptionForCompany(companyId);

            if (current != null &&
                current.SubscriptionPlanId == planId)
            {
                current.EndDate =
                    billingCycle == "Yearly"
                    ? current.EndDate!.Value.AddYears(1)
                    : current.EndDate!.Value.AddMonths(1);

                await SubscriptionService.UpdateAsync(current);
            }
            else
            {
                await SubscriptionService.CreateFromStripeAsync(
                    companyId,
                    planId,
                    billingCycle,
                    amount);
            }

            if (!string.IsNullOrEmpty(couponCode))
            {
                try
                {
                   
                    await couponService.ApplyAsync(new ApplyCouponDto
                    {
                        Code = couponCode,
                       
                        SubscriptionPlanId = planId 
                    });
                }
                catch
                {
                   
                }
            }

            return Ok();
        }



    }
}

