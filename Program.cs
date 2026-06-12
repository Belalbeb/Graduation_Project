
using Graduation_Project.Configurations;
using Graduation_Project.Middlewares;
using Graduation_Project.Models;
using Graduation_Project.Repositories;
using Graduation_Project.Seeds;


//using Graduation_Project.Seeds;
using Graduation_Project.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Stripe;
using System;
using System.Text;

namespace Graduation_Project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(new
                    {
                        errors,
                        status = 400
                    });
                };
            });
            builder.Services.Configure<CloudinarySettings>(
                 builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddScoped<CloudinaryService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
            builder.Services.AddScoped<IApplicantSkillRepository, ApplicantSkillRepository>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
            builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
            builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
            builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
            builder.Services.AddScoped<IAdminServices, AdminServices>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            builder.Services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<ISubscriptionService, Services.SubscriptionService>();
            builder.Services.AddScoped<IResumeServices, ResumeService>();

            builder.Services.AddScoped<Icouponrepository, CouponRepository>();
            builder.Services.AddScoped<ICouponService, Services.CouponService>();

            builder.Services.AddScoped<IApplicantServices, ApplicantServices>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<ICompanyServices, ComapnyServices>();
            builder.Services.AddScoped<IJobPostingService,JobPostingService>();
            builder.Services.AddScoped<IApplicationServivces, ApplicationServices>();
            builder.Services.AddScoped<IInterviewServices, InterviewService>() ;
            builder.Services.AddScoped<IExperienceService, ExperienceServices>() ;
            builder.Services.AddScoped<IApplicantSkillService, ApplicantSkillService>() ;
            builder.Services.AddScoped<ISettingsService, SettingsServices>() ;
            builder.Services.AddScoped<IProjectService, ProjectService>() ;
            builder.Services.AddScoped<ICompanySettingsService,CompanySettingsService>();
            builder.Services.AddScoped<ICompanyInterviewService,CompanyInterviewService>();
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();

            app.UseAuthorization();
            app.MapScalarApiReference();

            app.MapControllers();
            //using (var scope = app.Services.CreateScope())
            //{
            //    await SeederRunner.Run(scope.ServiceProvider);
            //}

            app.Run();
        }
    }
}
