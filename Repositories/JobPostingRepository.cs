using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Graduation_Project.Repositories
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly ApplicationDbContext _context;

        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<JobPosting>> GetAllAsync()
        {
            return await _context.JobPostings.ToListAsync();
        }

        public async Task<(IEnumerable<JobPosting> Jobs, int TotalCount)> GetAllAsync(JobFilterDto filter)
        {
            var query = _context.JobPostings.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(j =>
                    j.Title.Contains(filter.Title));
            }

            if (!string.IsNullOrWhiteSpace(filter.Location))
            {
                query = query.Where(j =>
                    j.Location.Contains(filter.Location));
            }

            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                query = query.Where(j =>
                    j.JobCategory.Contains( filter.Category));
            }
            var totalCount = await query.CountAsync();
            var jobs = await query
                 .OrderByDescending(j => j.PostedDate)
                 .Skip((filter.Page - 1) * filter.PageSize)
                 .Take(filter.PageSize)
                 .ToListAsync();
            return (jobs, totalCount);

        }
        public async Task<int> TotalJobs()
        {
            var jobs= await _context.JobPostings.ToListAsync();

            return jobs.Count();
        }
        public async Task<int> GetRejectedJobsCount()
        {
            var RejectedJobs= await _context.JobPostings.Where(x => x.Status == JobStatus.Rejected).ToListAsync();
            return RejectedJobs.Count();
        }

        public async Task<JobPosting?> GetByIdAsync(Guid id)
        {
            return await _context.JobPostings
                .Include(j => j.Company)
                .Include(i=>i.Skills)
                .Include(j => j.Applications)
                  .ThenInclude(j=>j.Applicant)
                    .ThenInclude(j=>j.Resumes)
                .FirstOrDefaultAsync(j => j.JobID == id);
        }
        public async Task<bool> AcceptJobAsync(Guid jobId)
        {
            var job = await _context.JobPostings.FirstOrDefaultAsync(x => x.JobID == jobId);
            if (job == null) return false;
            job.Status = JobStatus.Approved;
           await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RejectJobAsync(Guid jobId)
        {
            var job = await _context.JobPostings.FirstOrDefaultAsync(x => x.JobID == jobId);
            if (job == null) return false;
            job.Status = JobStatus.Rejected;
            await _context.SaveChangesAsync();
            return true;
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
        public async Task ReplaceSkillsAsync(Guid jobId, List<string> skills)
        {
            var existingSkills = _context.JobSkills
                .Where(s => s.JobPostingId == jobId);

            _context.JobSkills.RemoveRange(existingSkills);

            var newSkills = skills.Select(s => new JobSkill
            {
                Id = Guid.NewGuid(),
                Name = s,
                JobPostingId = jobId
            });

            await _context.JobSkills.AddRangeAsync(newSkills);
        }
        public async Task<bool> UpdateAsync( JobPosting jobPosting)
        {
          

            _context.JobPostings.Update(jobPosting);
            await _context.SaveChangesAsync();
            return true;
            
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

        public async Task<int> GetActiveJobsCountAsync()
        {
            return await _context.JobPostings.Where(x => x.Status==JobStatus.Approved).CountAsync();
        }

        public async Task<int> GetPendingJobsCountAsync()
        {
            return await _context.JobPostings.Where(x => x.Status==JobStatus.Pending).CountAsync();
        }

        public async Task<List<JobPosting>> GetLatestJobsAsync(int limit)
        {
            return await _context.JobPostings.Include(x=>x.Applications).Take(limit).ToListAsync();
        }

        public async Task<List<JobPosting>> GetPendingApprovalsAsync()
        {
            return await _context.JobPostings.Where(x => x.Status == JobStatus.Pending).Include(x=>x.Company).OrderBy(x=>x.PostedDate).Take(5).ToListAsync();
        }
        public Task<List<MonthlyStats>> GetMonthlyStatsAsync()
        {
            return _context.JobPostings
                .GroupBy(x => new { x.PostedDate.Year, x.PostedDate.Month })
                .Select(x => new MonthlyStats
                {
                    year = x.Key.Year,
                    month = x.Key.Month,
                    JobCount = x.Count(),
                    ApplicationCount = x.Sum(j => j.Applications.Count)
                })
                .ToListAsync();
        }
    }
}
