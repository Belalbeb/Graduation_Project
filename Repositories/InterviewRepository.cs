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

        public async Task<int> CountByApplicantAsync(int applicantId)
        {
            return await _context.Interviews.CountAsync(i => i.ApplicantId == applicantId);
        }

        public async Task<int> CountByApplicantAndStatusAsync(int applicantId, InterviewStatus status)
        {
            return await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.Status == status);
        }

        public async Task<List<Interview>> GetByApplicantAndStatusAsync(int applicantId, InterviewStatus status)
        {
            return await _context.Interviews
                .Where(i => i.ApplicantId == applicantId && i.Status == status)
                .Include(i => i.JobPosting)
                    .ThenInclude(j => j.Company)
                .OrderBy(i => i.ScheduledAt)
                .ToListAsync();
        }

        public async Task<List<Interview>> GetAllByApplicantAsync(int applicantId)
        {
            return await _context.Interviews
                .Where(i => i.ApplicantId == applicantId)
                .Include(i => i.JobPosting)
                    .ThenInclude(j => j.Company)
                .OrderBy(i => i.ScheduledAt)
                .ToListAsync();
        }
    }
}
