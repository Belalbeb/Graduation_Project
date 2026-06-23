using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region CompanyRole
        public async Task<Company> AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            return await _context.Companies
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CompanyID == id);
        }

        public async Task<Company?> GetByUserIdAsync(string userId)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Company?> GetWithJobPostingsAndApplicationsAsync(Guid companyId)
        {
            return await _context.Companies
                .Include(c => c.JobPostings)
                    .ThenInclude(jp => jp.Applications)
                        .ThenInclude(a => a.Applicant)
                .Include(c => c.JobPostings)
                    .ThenInclude(jp => jp.Interviews)
                .FirstOrDefaultAsync(c => c.CompanyID == companyId);
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            if(company == null) return false;

            _context.Companies.Remove(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Company?> GetCompanyForSettingsAsync(Guid companyId)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.CompanyID == companyId);
        }

        public async Task<bool> UpdateCompanyProfileAsync(Company company)
        {
            _context.Companies.Update(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetCompaniesCount()
        {
            return await _context.Companies.CountAsync();
        }

        public async Task<List<JobPosting>> GetJobPostingsByCompanyIdAsync(Guid companyId)
        {
            return await _context.JobPostings
                .Where(j => j.CompanyID == companyId)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        #endregion
        // ======================= Admin =========================
        // Get all companies for admin (with User and JobPostings)
        public async Task<List<Company>> GetAllCompaniesForAdminAsync()
        {
            return await _context.Companies
                .Include(c => c.User)
                .Include(c => c.JobPostings)
                .OrderByDescending(c => c.User.CreatedAt)
                .ToListAsync();
        }

        // Get single company with all details
        public async Task<Company?> GetCompanyWithDetailsForAdminAsync(Guid companyId)
        {
            return await _context.Companies
                .Include(c => c.User)
                .Include(c => c.JobPostings)
                    .ThenInclude(j => j.Applications)
                .Include(c => c.JobPostings)
                    .ThenInclude(j => j.Interviews)
                .FirstOrDefaultAsync(c => c.CompanyID == companyId);
        }

        // Count total companies
        public async Task<int> CountTotalCompaniesAsync()
        {
            return await _context.Companies.CountAsync();
        }

        // Count verified companies
        public async Task<int> CountVerifiedCompaniesAsync()
        {
            return await _context.Companies.CountAsync(c => c.Status == CompanyStatus.Verified);
        }

        // Count pending companies (verification requests)
        public async Task<int> CountPendingCompaniesAsync()
        {
            return await _context.Companies.CountAsync(c => c.Status == CompanyStatus.Active);
        }

        // Count total jobs for a company
        public async Task<int> CountCompanyJobsAsync(Guid companyId)
        {
            return await _context.JobPostings.CountAsync(j => j.CompanyID == companyId);
        }

        // Count active jobs for a company
        public async Task<int> CountCompanyActiveJobsAsync(Guid companyId)
        {
            return await _context.JobPostings
                .CountAsync(j => j.CompanyID == companyId && j.IsActive && j.Status == JobStatus.Approved);
        }

        // Count total applicants (applications) for a company
        public async Task<int> CountCompanyApplicantsAsync(Guid companyId)
        {
            return await _context.Applications
                .CountAsync(a => a.JobPosting.CompanyID == companyId);
        }

        // Count total interviews for a company
        public async Task<int> CountCompanyInterviewsAsync(Guid companyId)
        {
            return await _context.Interviews
                .CountAsync(i => i.JobPosting.CompanyID == companyId);
        }

        // Get company's current active subscription plan name
        public async Task<string?> GetCompanyActiveSubscriptionPlanAsync(Guid companyId)
        {
            var activeSubscription = await _context.companySubscriptions
                .Include(s => s.SubscriptionPlan)
                .Where(s => s.CompanyId == companyId && s.IsActive && s.EndDate > DateTime.UtcNow)
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefaultAsync();

            if(activeSubscription == null)
                return "Free";

            return activeSubscription.SubscriptionPlan?.Name ?? "Free";
        }
    }
}