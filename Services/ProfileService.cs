using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(IProfileRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository   = repository;
            _userManager  = userManager;
        }

        public async Task<(string profileId, string profileType)> GetProfileAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role  = roles.FirstOrDefault();

            if (role == null) return (null, null);

            switch (role)
            {
                case Roles.Applicant:
                    var applicant = await _repository.GetApplicantByUserIdAsync(user.Id);
                    return (applicant?.ApplicantID.ToString(), "Applicant");

                case Roles.Company:
                    var company = await _repository.GetCompanyByUserIdAsync(user.Id);
                    return (company?.CompanyID.ToString(), "Company");

                default:
                    return (null, role);
            }
        }

        public async Task<PublicProfileDto?> GetPublicProfileAsync(Guid applicantId)
        {
            var applicant = await _repository.GetPublicProfileAsync(applicantId);
            if (applicant == null) return null;

            return new PublicProfileDto
            {
                ApplicantID = applicant.ApplicantID,
                FullName = $"{applicant.FirstName} {applicant.LastName}".Trim(),
                JobTitle = applicant.JobTitle ?? string.Empty,
                Location = applicant.Location ?? string.Empty,
                AboutMe = applicant.AboutMe,
                Industry=applicant.Industry,
                ProfilePicUrl = applicant.ProfilePicURL,
                CoverPhotoUrl = applicant.CoverPhotoUrl,
                Email = applicant.User.Email,
                PhoneNumber = applicant.PhoneNumber,
                Address = applicant.Address,
                Linkedin = applicant.Linkedin,
                Github = applicant.Github,
                Facebook = applicant.Facebook,
                Portfolio = applicant.Portfolio,

                Behance = applicant.Behance,
                Dribbble = applicant.Dribble,
                Resumes = applicant.Resumes.Select(x =>  new ResumeDto {ResumeId=x.ResumeID, Name = x.FileName, Url = x.FilePath }).ToList(),

                Experiences = applicant.Experiences
                    .OrderByDescending(e => e.StartDate)
                    .Select(e => new ExperienceResponseDto
                    {
                        ExperienceID = e.ExperienceID,
                        CompanyName = e.CompanyName,
                        Location = e.Location ?? "",
                        JobTitle = e.JobTitle,
                        Description = e.Description,
                        JobType = e.JobType.ToString(),
                        StartDate = e.StartDate,
                        EndDate = e.EndDate

                    }).ToList(),

                Skills = applicant.ApplicantSkills
                    .Select(s => new SkillResponseDto
                    {
                        ApplicantSkillID = s.ApplicantSkillID,
                        SkillID = s.SkillID,
                        SkillName = s.Skill.SkillName
                    }).ToList(),

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
                    }).ToList()
            };
        }
    }
}
