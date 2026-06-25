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

        public async Task<List<ApplicationDto>> GetByApplicantIdAsync(Guid applicantId)
        {
            return await _context.Applications
                .Where(x => x.ApplicantID == applicantId)
                .Select(x => new ApplicationDto
                {
                    ApplicationId = x.ApplicationID,
                    JobType = x.JobPosting.JobTypes.Select(x=>x.ToString()).ToList(),
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
        public async Task<Application> GetApplicationByIdAsync(Guid ApplicationId)
        {
            return await _context.Applications.Include(x=>x.Applicant)
                .ThenInclude(x=>x.User)
                .Include(x=>x.Resume).FirstOrDefaultAsync(x => x.ApplicationID == ApplicationId);

        }
        public async Task<bool> ChangeApplicationStatus(Guid ApplicationId,Guid companyId ,ApplicationStatus status)
        {
            var Application =await _context.Applications.FirstOrDefaultAsync(x => x.ApplicationID == ApplicationId &&x.JobPosting.CompanyID==companyId);
            if (Application == null) return false;
            Application.ApplicationStatus = status;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task AddApplication(Application application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
            ;
        }
    }
}
