using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountByApplicantAsync(Guid applicantId)
        {
            return await _context.Interviews.CountAsync(i => i.ApplicantId == applicantId);
        }

        public async Task<int> CountByApplicantAndStatusAsync(Guid applicantId, InterviewStatus status)
        {
            return await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.Status == status);
        }

        public async Task<List<Interview>> GetByApplicantAndStatusAsync(Guid applicantId, InterviewStatus status)
        {
            return await _context.Interviews
                .Where(i => i.ApplicantId == applicantId && i.Status == status)
                .Include(i => i.JobPosting)
                    .ThenInclude(j => j.Company)
                .OrderBy(i => i.ScheduledAt)
                .ToListAsync();
        }

        public async Task<List<Interview>> GetAllByApplicantAsync(Guid applicantId)
        {
            return await _context.Interviews
                .Where(i => i.ApplicantId == applicantId)
                .Include(i => i.JobPosting)
                    .ThenInclude(j => j.Company)
                .OrderBy(i => i.ScheduledAt)
                .ToListAsync();
        }
        public async Task<List<Interview>> GetByjobPostingId(Guid jobId)
        {
            return await _context.Interviews.Where(i => i.JobPostingId == jobId).Include(x=>x.Applicant).ToListAsync();
        }
        public async Task<Interview> GetInterviewById(Guid InterviewId)
        {
            return await _context.Interviews.Include(x=>x.JobPosting).Include(x=>x.Applicant)
                .ThenInclude(x=>x.Resumes).FirstOrDefaultAsync(i => i.InterviewId == InterviewId);

        }

        public async Task<bool> ChangeInterviewDate(Guid InterviewId,DateTime InterviewDate)
        {
            var Interview =await _context.Interviews.FirstOrDefaultAsync(x => x.InterviewId == InterviewId);

            if (Interview == null) return false;
            Interview.ScheduledAt = InterviewDate;
           await _context.SaveChangesAsync();
            return true;

        }
    }
}
