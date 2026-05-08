using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly ApplicationDbContext _context;

        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsync()
        {
            return await _context.JobPostings.Include(x => x.Company).ToListAsync();
        }

        public async Task<JobPosting?> GetByIdAsync(Guid id)
        {
            return await _context.JobPostings
                .Include(j => j.Company)
                .Include(j => j.Applications)
                .FirstOrDefaultAsync(j => j.JobID == id);
        }

        public async Task<IEnumerable<JobPosting>> GetByCompanyAsync(Guid companyId)
        {
            return await _context.JobPostings
                .Include(x=>x.Applications)
                  
                .Where(x => x.CompanyID == companyId).ToListAsync();
        }

        public async Task<JobPosting> AddAsync(JobPosting jobPosting)
        {
            jobPosting.PostedDate = DateTime.Now;
            jobPosting.IsActive = true;
            await _context.JobPostings.AddAsync(jobPosting);
            await _context.SaveChangesAsync();
            return jobPosting;
        }

        public async Task<JobPosting?> UpdateAsync(Guid id, JobPosting jobPosting)
        {
            var existing = await _context.JobPostings.FindAsync(id);
            if (existing == null) return null;

            existing.Title = jobPosting.Title;
            existing.Description = jobPosting.Description;
            existing.Responsibility = jobPosting.Responsibility;
            existing.MinSalary = jobPosting.MinSalary;
            existing.MaxSalary = jobPosting.MaxSalary;
            existing.IsActive = jobPosting.IsActive;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var job = await _context.JobPostings.FindAsync(id);
            if (job == null) return false;

            _context.JobPostings.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateAsync(Guid id)
        {
            var job = await _context.JobPostings.FindAsync(id);
            if (job == null) return false;

            job.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
