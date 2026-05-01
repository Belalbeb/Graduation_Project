using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class InterviewService : IInterviewServices
    {
        private readonly ApplicationDbContext _context ;

        public InterviewService(ApplicationDbContext context)
        {
            _context = context ;
        }

        public async Task<InterviewStatisticsDto> GetStatisticsAsync(int applicantId)
        {
            var total = await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId);

            var upcoming = await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.Status == InterviewStatus.Upcoming);

            var completed = await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.Status == InterviewStatus.Completed);

            var cancelled = await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.Status == InterviewStatus.Cancelled);

            return new InterviewStatisticsDto
            {
                Total = total,
                Upcoming = upcoming,
                Completed = completed,
                Cancelled = cancelled
            };
        }

        public async Task<List<InterviewResponseDto>> GetUpcomingAsync(int applicantId)
        {
            return await GetInterviewsByStatusAsync(applicantId,InterviewStatus.Upcoming);
        }

        public async Task<List<InterviewResponseDto>> GetCompletedAsync(int applicantId)
        {
            return await GetInterviewsByStatusAsync(applicantId,InterviewStatus.Completed);
        }

        public async Task<List<InterviewResponseDto>> GetCancelledAsync(int applicantId)
        {
            return await GetInterviewsByStatusAsync(applicantId,InterviewStatus.Cancelled);
        }

        public async Task<List<InterviewResponseDto>> GetAllAsync(int applicantId)
        {
            return await _context.Interviews
                .Where(i => i.ApplicantId == applicantId)
                .Include(i => i.JobPosting)
                    .ThenInclude(j => j.Company)
                    .OrderBy(i => i.ScheduledAt)
                    .Select(i => new InterviewResponseDto
                    {
                        InterviewId = i.InterviewId,
                        JobPostingId = i.JobPostingId,
                        JobTitle = i.JobPosting.Title,
                        CompanyName = i.JobPosting.Company.Name,
                        CompanyLogoUrl = i.JobPosting.Company.LogoUrl,
                        ScheduledAt = i.ScheduledAt,
                        Status = i.Status,
                        InterviewerName = i.InterviewerName,
                        MeetingLink = i.MeetingLink,
                        Notes = i.Notes
                    }).ToListAsync() ;
        }

        private async Task<List<InterviewResponseDto>> GetInterviewsByStatusAsync(int applicantId,InterviewStatus? status)
        {
            var query = _context.Interviews
                .Where(i => i.ApplicantId == applicantId)
                .Include(i => i.JobPosting)
                    .ThenInclude(j => j.Company)
                    .Where(i => i.Status == status.Value);

            return await query
                .OrderBy(i => i.ScheduledAt)
                .Select(i => new InterviewResponseDto
                {
                    InterviewId = i.InterviewId,
                    JobPostingId = i.JobPostingId,
                    JobTitle = i.JobPosting.Title,
                    CompanyName = i.JobPosting.Company.Name,
                    CompanyLogoUrl = i.JobPosting.Company.LogoUrl,
                    ScheduledAt = i.ScheduledAt,
                    Status = i.Status,
                    InterviewerName = i.InterviewerName,
                    MeetingLink = i.MeetingLink,
                    Notes = i.Notes
                })
                .ToListAsync();
        }
    }

}
