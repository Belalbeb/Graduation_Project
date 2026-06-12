using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IApplicantServices applicantServices,
            ICompanyServices companyServices,
            IConfiguration configuration,
            IProfileService profileService)
        {
            this.userManager = userManager;
            ApplicantServices = applicantServices;
            CompanyServices = companyServices;
            this.configuration = configuration;
            this.profileService = profileService;
        }


        [HttpGet("user-details")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId =(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user =await userManager.FindByIdAsync(userId);
            var roles = await userManager.GetRolesAsync(user);
            string? photoUrl = null;

            if (roles.Contains(Roles.Applicant))
            {
                var applicant = await ApplicantServices.GetApplicantByUserIdAsync(userId);
                photoUrl = applicant?.ProfilePicURL;
            }
            else if (roles.Contains(Roles.Company))
            {
                var company = await CompanyServices.GetCompanyByUserIdAsync(userId);
                photoUrl = company?.LogoUrl; 
            }

            return Ok(new { userId = userId, email = user.Email, roles = roles,photoUrl });


        }
        [HttpPost("register/applicant")]
        public async Task<IActionResult> Register(
            [FromBody] ApplicantRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                CreatedAt = DateTime.Now
            };

            var result = await userManager.CreateAsync(
                user,
                model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e =>
                {
                    return e.Code switch
                    {
                        "DuplicateUserName" => "Email already exists",
                        "PasswordTooShort" => "Password is too weak",
                        _ => e.Description
                    };
                }).ToList();

                return BadRequest(new
                {
                    errors,
                    status = 400
                });
            }

            await userManager.AddToRoleAsync(user, Roles.Applicant);

            Applicant applicant = new Applicant()
            {
                UserId = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Location = model.Location,
            };

            await ApplicantServices.CreateApplicantAsync(applicant);

            var token = await GenerateJwtToken(user);

            return Ok(token);
        }

        [HttpPost("register/company")]
        public async Task<IActionResult> Register(
            [FromBody] CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser()
            {
                UserName = companyDto.Email,
                Email = companyDto.Email,
                CreatedAt = DateTime.Now
            };

            var result = await userManager.CreateAsync(
                user,
                companyDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e =>
                {
                    return e.Code switch
                    {
                        "DuplicateUserName" => "Email already exists",
                        "PasswordTooShort" => "Password is too weak",
                        _ => e.Description
                    };
                }).ToList();

                return BadRequest(new
                {
                    errors,
                    status = 400
                });
            }

            await userManager.AddToRoleAsync(user, Roles.Company);

            Company company = new Company()
            {
                UserId = user.Id,
                Name = companyDto.Name,
                Industry = companyDto.Industry,
                Location = companyDto.Location
            };

            await CompanyServices.AddCompanyAsync(company);

            var token = await GenerateJwtToken(user);

            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    error = "Invalid username or password"
                });

            var user = await userManager.FindByEmailAsync(
                loginDto.Email);

            if (user == null)
                return Unauthorized(new
                {
                    error = "Invalid username or password"
                });

            bool verifyPass = await userManager.CheckPasswordAsync(
                user,
                loginDto.password);

            if (!verifyPass)
            {
                return Unauthorized(new
                {
                    error = "Invalid username or password"
                });
            }

            var token = await GenerateJwtToken(user);

            return Ok(token);
        }
     

        private async Task<object> GenerateJwtToken(
            ApplicationUser user)
        {
            var (profileId, profileType) =
                await profileService.GetProfileAsync(user);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (profileId != null)
                claims.Add(new Claim(
                    CustomClaims.ProfileId,
                    profileId));

            if (profileType != null)
                claims.Add(new Claim(
                    CustomClaims.ProfileType,
                    profileType));

            var key = configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(key))
                throw new Exception("JWT Key is missing");

            SymmetricSecurityKey symmetricSecurityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            SigningCredentials signingCredentials =
                new SigningCredentials(
                    symmetricSecurityKey,
                    SecurityAlgorithms.HmacSha512);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials
            );

            return new
            {
                token = new JwtSecurityTokenHandler()
                    .WriteToken(token),

                expiration = token.ValidTo,

                userId = user.Id,

                email = user.Email,

                roles = roles
            };
        }
    }
}