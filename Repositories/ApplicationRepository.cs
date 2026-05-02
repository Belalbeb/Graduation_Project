using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationDto>> GetByApplicantIdAsync(int applicantId)
        {
            return await _context.Applications
                .Where(x => x.ApplicantID == applicantId)
                .Select(x => new ApplicationDto
                {
                    ApplicationId = x.ApplicationID,
                    JobType = x.JobPosting.JobType.ToString(),
                    ApplicationStatus = x.ApplicationStatus.ToString(),
                    AppliedOn = x.AppliedDate,
                    JobTitle = x.JobPosting.Title,
                    CompanyName = x.JobPosting.Company.Name,
                    Location = x.JobPosting.Company.Location,
                    LogoUrl = x.JobPosting.Company.LogoUrl,
                    IsRemote = x.JobPosting.IsRemote
                })
                .ToListAsync();
        }
    }
}
