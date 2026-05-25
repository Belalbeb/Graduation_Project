using Graduation_Project.Dtos;
using Graduation_Project.Dtos.Company.Interview;
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

        // ==================== Applicant Methods ====================
        public async Task<int> CountByApplicantAsync(Guid applicantId)
        {
            return await _context.Interviews.CountAsync(i => i.ApplicantId == applicantId);
        }

        public async Task<int> CountByApplicantAndStatusAsync(Guid applicantId,InterviewStatus status)
        {
            return await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.Status == status);
        }

        public async Task<List<Interview>> GetByApplicantAndStatusAsync(Guid applicantId,InterviewStatus status)
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

        // ==================== Company Methods ====================
        public async Task<CompanyInterviewStatisticsDto> GetCompanyStatisticsAsync(Guid companyId)
        {
            var now = DateTime.UtcNow.Date;

            var total = await _context.Interviews
                .CountAsync(i => i.JobPosting.CompanyID == companyId);

            var today = await _context.Interviews
                .CountAsync(i => i.JobPosting.CompanyID == companyId &&
                                 i.ScheduledAt.Date == now);

            var completed = await _context.Interviews
                .CountAsync(i => i.JobPosting.CompanyID == companyId &&
                                 i.Status == InterviewStatus.Completed);

            var pending = await _context.Interviews
                .CountAsync(i => i.JobPosting.CompanyID == companyId &&
                                 (i.Status == InterviewStatus.Pending || i.Status == InterviewStatus.Upcoming));

            return new CompanyInterviewStatisticsDto
            {
                TotalInterviews = total,
                TodaysInterviews = today,
                CompletedInterviews = completed,
                PendingInterviews = pending
            };
        }

        public async Task<List<Interview>> GetCompanyInterviewsAsync(Guid companyId,string? search = null,string? status = null)
        {
            var query = _context.Interviews
                .Include(i => i.Applicant)
                .Include(i => i.JobPosting)
                .Where(i => i.JobPosting.CompanyID == companyId);

            if(!string.IsNullOrEmpty(status))
            {
                if(Enum.TryParse<InterviewStatus>(status,true,out var statusEnum))
                {
                    query = query.Where(i => i.Status == statusEnum);
                }
            }

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(i => i.Applicant.FirstName.Contains(search) ||
                                         i.Applicant.LastName.Contains(search));
            }

            return await query
                .OrderByDescending(i => i.ScheduledAt)
                .ToListAsync();
        }

        public async Task<Interview?> GetCompanyInterviewByIdAsync(Guid interviewId,Guid companyId)
        {
            return await _context.Interviews
                .Include(i => i.Applicant)
                .Include(i => i.JobPosting)
                .FirstOrDefaultAsync(i => i.InterviewId == interviewId &&
                                          i.JobPosting.CompanyID == companyId);
        }

        public async Task UpdateAsync(Interview interview)
        {
            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Interview>> GetByjobPostingId(Guid jobId)
        {
            return await _context.Interviews
                .Where(i => i.JobPostingId == jobId)
                .Include(x => x.Applicant)
                .ToListAsync();
        }

        public async Task<Interview> GetInterviewById(Guid InterviewId)
        {
            return await _context.Interviews
                .Include(x => x.JobPosting)
                .Include(x => x.Applicant)
                    .ThenInclude(x => x.Resumes)
                .FirstOrDefaultAsync(i => i.InterviewId == InterviewId);
        }

        public async Task<bool> ChangeInterviewDate(Guid InterviewId,DateTime InterviewDate)
        {
            var Interview = await _context.Interviews.FirstOrDefaultAsync(x => x.InterviewId == InterviewId);
            if(Interview == null) return false;

            Interview.ScheduledAt = InterviewDate;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}