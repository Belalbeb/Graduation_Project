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
        public async Task<List<Interview>?> GetUpcomingInterviewsAsync(int applicantId)
        {
            return await _context.Interviews
                .Where(In => In.ApplicantId == applicantId && In.Status == InterviewStatus.Upcoming)
                .ToListAsync() ;
        }

        public async Task<List<Interview>?> GetCompletedInterviewsAsync(int applicantId)
        {
            return await _context.Interviews
                .Where(In => In.ApplicantId == applicantId && In.Status == InterviewStatus.Completed)
                .ToListAsync();
        }
        public async Task<List<Interview>?> GetCancelledInterviewsAsync(int applicantId)
        {
            return await _context.Interviews
                .Where(In => In.ApplicantId == applicantId && In.Status == InterviewStatus.Cancelled)
                .ToListAsync();
        }

    }
}
