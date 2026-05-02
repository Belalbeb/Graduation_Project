using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Applicant> CreateAsync(Applicant applicant)
        {
            await _context.Applicants.AddAsync(applicant);
            await _context.SaveChangesAsync();
            return applicant;
        }

        public async Task<Applicant?> GetByIdAsync(int id)
        {
            return await _context.Applicants.FirstOrDefaultAsync(x => x.ApplicantID == id);
        }

        public async Task<Applicant?> GetByUserIdAsync(string userId)
        {
            return await _context.Applicants.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<List<Applicant>> GetAllAsync()
        {
            return await _context.Applicants.ToListAsync();
        }

        public async Task UpdateAsync(Applicant applicant)
        {
            _context.Applicants.Update(applicant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Applicant applicant)
        {
            _context.Applicants.Remove(applicant);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountApplicationsAsync(int applicantId)
        {
            return await _context.Applications.CountAsync(a => a.ApplicantID == applicantId);
        }

        public async Task<int> CountSavedJobsAsync(int applicantId)
        {
            return await _context.SavedJobs.CountAsync(s => s.ApplicantId == applicantId);
        }

        public async Task<int> CountUpcomingInterviewsAsync(int applicantId)
        {
            return await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.ScheduledAt >= DateTime.UtcNow);
        }

        public async Task<int> CountProfileViewsAsync(int applicantId)
        {
            return await _context.ProfileViews.CountAsync(p => p.ApplicantId == applicantId);
        }

        public async Task<List<dynamic>> GetMonthlyApplicationsAsync(int applicantId, DateTime startOfYear)
        {
            return (await _context.Applications
                .Where(a => a.ApplicantID == applicantId && a.AppliedDate >= startOfYear)
                .GroupBy(a => a.AppliedDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync())
                .Cast<dynamic>()
                .ToList();
        }

        public async Task<List<dynamic>> GetMonthlyInterviewsAsync(int applicantId, DateTime startOfYear)
        {
            return (await _context.Interviews
                .Where(i => i.ApplicantId == applicantId && i.ScheduledAt >= startOfYear)
                .GroupBy(i => i.ScheduledAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync())
                .Cast<dynamic>()
                .ToList();
        }

        public async Task<List<RecentApplicationDto>> GetRecentApplicationsAsync(int applicantId, int take)
        {
            return await _context.Applications
                .Where(a => a.ApplicantID == applicantId)
                .OrderByDescending(a => a.AppliedDate)
                .Take(take)
                .Select(a => new RecentApplicationDto
                {
                    Id = a.ApplicationID,
                    CompanyName = a.JobPosting.Company.Name,
                    CompanyLogoUrl = a.JobPosting.Company.LogoUrl,
                    JobTitle = a.JobPosting.Title,
                    AppliedAt = a.AppliedDate
                })
                .ToListAsync();
        }

        public async Task<List<SavedJobsResponseDto>> GetSavedJobsAsync(int applicantId)
        {
            return await _context.SavedJobs
                .Where(x => x.ApplicantId == applicantId)
                .Select(item => new SavedJobsResponseDto
                {
                    JobTitle = item.JobPosting.Title,
                    JobDescription = item.JobPosting.Description,
                    JobRequirement = item.JobPosting.Requirements,
                    SavedAt = item.SavedAt,
                    CompanyLogoUrl = item.JobPosting.Company.LogoUrl,
                    CompanyLocation = item.JobPosting.Company.Location,
                    CompanyName = item.JobPosting.Company.Name,
                    JobType = item.JobPosting.JobType.ToString(),
                    SalaryRange = item.JobPosting.SalaryRange
                })
                .ToListAsync();
        }

        public async Task DeactivateAllResumesAsync(int applicantId)
        {
            await _context.Resumes
                .Where(r => r.ApplicantID == applicantId)
                .ExecuteUpdateAsync(s => s.SetProperty(r => r.IsActive, false));
        }

        public async Task AddResumeAsync(Resume resume)
        {
            await _context.Resumes.AddAsync(resume);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> GetActiveResumePathAsync(int applicantId)
        {
            return await _context.Resumes
                .Where(r => r.ApplicantID == applicantId && r.IsActive)
                .Select(r => r.FilePath)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetActiveResumePathByUserIdAsync(string userId)
        {
            return await _context.Applicants
                .Where(a => a.UserId == userId)
                .SelectMany(a => a.Resumes)
                .Where(r => r.IsActive)
                .Select(r => r.FilePath)
                .FirstOrDefaultAsync();
        }
    }
}
