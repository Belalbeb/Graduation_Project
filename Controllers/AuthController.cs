using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IProfileService profileService;

        public IApplicantServices ApplicantServices { get; }
        public ICompanyServices CompanyServices { get; }

        public AuthController(UserManager<ApplicationUser> userManager,IApplicantServices applicantServices,ICompanyServices companyServices,IConfiguration configuration,IProfileService profileService)
        {
            this.userManager = userManager;
            ApplicantServices = applicantServices;
            CompanyServices = companyServices;
            this.configuration = configuration;
            this.profileService = profileService;
        }
        [HttpPost("register/applicant")]
        public async Task<IActionResult> Register([FromBody] ApplicantRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = new ApplicationUser()
            {
                UserName=model.Email,
                Email = model.Email,
                CreatedAt=DateTime.Now

            };
         var result=await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

           await userManager.AddToRoleAsync(user, Roles.Applicant);

            Applicant applicant = new Applicant()
            {
                UserId=user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Location = model.Location,
                PhoneNumber = model.PhoneNumber
            };
            await ApplicantServices.CreateApplicantAsync(applicant);
            return Ok(new { message = "Applicant registered successfully" });


        }
        [HttpPost("register/company")]
        public async Task<IActionResult> Register(CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = new ApplicationUser()
            {
                UserName = companyDto.Email,
                Email = companyDto.Email,
                CreatedAt = DateTime.Now

            };
          var result=  await userManager.CreateAsync(user,companyDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

   
            await userManager.AddToRoleAsync(user, "Company");
            Company company = new Company()
            {
                UserId=user.Id,
                Name = companyDto.Name,
                WebsiteURL = companyDto.WebsiteURL,
                Industry = companyDto.Industry,
                HeadquarterAddress = companyDto.HeadquarterAddress,
                Location=companyDto.Location,
                LogoUrl=companyDto.Logo

            };
            await CompanyServices.AddCompanyAsync(company);
            return Ok(new {message="company register successfully"});

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid username or password");
           var user = (userManager.FindByEmailAsync(loginDto.Email)).Result;
            if (user == null)
                return NotFound("user Not found");
         bool VerfiyPass=   await userManager.CheckPasswordAsync(user, loginDto.password);
            if (VerfiyPass)
            {
                //generate token
                var (profileId, profileType) = await profileService.GetProfileAsync(user);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                var roles = await userManager.GetRolesAsync(user);
                foreach (var item in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));

                }
                if (profileId != null)
                    claims.Add(new Claim(CustomClaims.ProfileId, profileId));

                if (profileType != null)
                    claims.Add(new Claim(CustomClaims.ProfileType, profileType));

                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials:signingCredentials

                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userId=user.Id
                });
            }
            else
            {
                return Unauthorized("Incorrect Username or password");
            }

        }

    }
}
