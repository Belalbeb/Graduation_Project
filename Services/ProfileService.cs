using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class ProfileService:IProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<(string profileId, string profileType)> GetProfileAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == null)
                return (null, null);

            switch (role)
            {
                case Roles.Applicant:
                    var applicant = await _context.Applicants
                        .FirstOrDefaultAsync(x => x.UserId == user.Id);

                    return (applicant?.ApplicantID.ToString(), "Applicant");

                //case "Company":
                //    var company = await _context.Companies
                //        .FirstOrDefaultAsync(x => x.UserId == user.Id);

                //    return (company?.Id.ToString(), "Company");

                //case "Admin":
                //    var admin = await _context.Admins
                //        .FirstOrDefaultAsync(x => x.UserId == user.Id);

                //    return (admin?.Id.ToString(), "Admin");

                default:
                    return (null, role);
            }
        }

        public async Task<PublicProfileDto?> GetPublicProfileAsync(int applicantId)
        {
            var applicant = await _context.Applicants
                .Include(a => a.Experiences)
                .Include(a => a.ApplicantSkills)
                    .ThenInclude(s => s.Skill)
                .Include(a => a.Projects)
                .FirstOrDefaultAsync(a => a.ApplicantID == applicantId);

            if (applicant == null)
                return null;

            return new PublicProfileDto
            {
                ApplicantID = applicant.ApplicantID,
                FullName = $"{applicant.FirstName} {applicant.LastName}".Trim(),
                JobTitle = applicant.JobTitle ?? string.Empty,
                Location = applicant.Location ?? string.Empty,
                AboutMe = applicant.AboutMe,
                ProfilePicUrl = applicant.ProfilePicURL,
                CoverPhotoUrl = applicant.CoverPhotoUrl,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                Linkedin = applicant.Linkedin,
                Github = applicant.Github,
                Facebook = applicant.Facebook,
                Portfolio = applicant.Portfolio,

                Experiences = applicant.Experiences
                    .OrderByDescending(e => e.StartDate)
                    .Select(e => new ExperienceResponseDto
                    {
                        ExperienceID = e.ExperienceID,
                        CompanyName = e.CompanyName,
                        Location = e.Location ?? "",
                        JobTitle = e.JobTitle,
                        Description = e.Description,
                        JobType = e.JobType,
                        StartDate = e.StartDate
                    })
                    .ToList(),

                Skills = applicant.ApplicantSkills
                    .Select(s => new SkillResponseDto
                    {
                        ApplicantSkillID = s.ApplicantSkillID,
                        SkillID = s.SkillID,
                        SkillName = s.Skill.SkillName
                    })
                    .ToList(),

                Projects = applicant.Projects
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new ProjectResponseDto
                    {
                        ProjectID = p.ProjectID,
                        Title = p.Title,
                        Description = p.Description,
                        ProjectUrl = p.ProjectUrl,
                        GithubRepoUrl = p.GithubRepoUrl,
                        ImageUrl = p.ImageUrl,
                        CreatedAt = p.CreatedAt
                    })
                    .ToList()
            };
        }

    }
}
