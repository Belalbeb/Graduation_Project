using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
namespace Graduation_Project.Repositories
{
    

    public class SavedJobRepository : ISavedJobRepository
    {
        private readonly ApplicationDbContext _context;

        public SavedJobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SavedJobs savedJob)
        {
            await _context.SavedJobs.AddAsync(savedJob);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(
            Guid applicantId,
            Guid jobId)
        {
            return await _context.SavedJobs.AnyAsync(x =>
                x.ApplicantId == applicantId &&
                x.JobPostingId == jobId);
        }
        public async Task UnsaveJobAsync( Guid applicantId,Guid jobId)
        {
            var savedJob = await _context.SavedJobs
                .FirstOrDefaultAsync(x =>
                    x.ApplicantId == applicantId &&
                    x.JobPostingId == jobId);

            if (savedJob == null)
                return;

            _context.SavedJobs.Remove(savedJob);

            await _context.SaveChangesAsync();
        }
        public async Task<List<SavedJobs>> GetSavedJobsAsync(
         Guid applicantId)
        {
            return await _context.SavedJobs
                .Include(x => x.JobPosting)
                    .ThenInclude(x => x.Company)
                .Include(x => x.JobPosting)
                    .ThenInclude(x => x.Applications)
                .Where(x => x.ApplicantId == applicantId)
                .ToListAsync();
        }
        // for get all jobs
        public async Task<List<Guid>> GetSavedJobIdsAsync(Guid applicantId)
        {
            return await _context.SavedJobs
                .Where(x => x.ApplicantId == applicantId)
                .Select(x => x.JobPostingId)
                .ToListAsync();
        }
    }
}
