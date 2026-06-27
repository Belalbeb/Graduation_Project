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

        public async Task<Applicant?> GetByIdAsync(Guid id)
        {
            return await _context.Applicants
                .Include(x=>x.Resumes)
                .Include(x=>x.User).
                FirstOrDefaultAsync(x => x.ApplicantID == id);
        }

        public async Task<Applicant?> GetByUserIdAsync(string userId)
        {
            return await _context.Applicants.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<List<Applicant>> GetAllAsync()
        {
            return await _context.Applicants.ToListAsync();
        }
        public async Task<(List<Applicant> Items, int TotalCount)> GetCandidatesAsync(
     int page,
     CandidateFilterDto filter)
        {
            const int pageSize = 20;

            if (page < 1)
                page = 1;

            var query = _context.Applicants
                .AsNoTracking()
                .Where(x => !x.IsBlocked)
                .AsQueryable();

          
            if (!string.IsNullOrWhiteSpace(filter?.CandidateName))
            {
                var name = filter.CandidateName.Trim().ToLower();

                query = query.Where(x =>
                    ((x.FirstName ?? "") + " " + (x.LastName ?? ""))
                    .ToLower()
                    .Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(filter?.Country))
            {
                var country = filter.Country.Trim().ToLower();

                query = query.Where(x =>
                    (x.Location ?? "")
                    .ToLower()
                    .Contains(country));
            }

     
            if (!string.IsNullOrWhiteSpace(filter?.Industry))
            {
                var industry = filter.Industry.Trim().ToLower();

                query = query.Where(x =>
                    (x.Industry ?? "")
                    .ToLower()
                    .Contains(industry));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.ApplicantID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        public async Task<int> CountActiveApplicant()
        {
            return await _context.Applicants.CountAsync(x=>!x.IsBlocked);
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

        public async Task<int> CountApplicationsAsync(Guid applicantId)
        {
            return await _context.Applications.CountAsync(a => a.ApplicantID == applicantId);
        }

        public async Task<int> CountSavedJobsAsync(Guid applicantId)
        {
            return await _context.SavedJobs.CountAsync(s => s.ApplicantId == applicantId);
        }

        public async Task<int> CountUpcomingInterviewsAsync(Guid applicantId)
        {
            return await _context.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.InterviewDate >= DateOnly.FromDateTime(DateTime.UtcNow));
        }

        public async Task<int> CountProfileViewsAsync(Guid applicantId)
        {
            return await _context.ProfileViews.CountAsync(p => p.ApplicantId == applicantId);
        }

        public async Task<List<dynamic>> GetMonthlyApplicationsAsync(Guid applicantId, DateTime startOfYear)
        {
            return (await _context.Applications
                .Where(a => a.ApplicantID == applicantId && a.AppliedDate >= startOfYear)
                .GroupBy(a => a.AppliedDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync())
                .Cast<dynamic>()
                .ToList();
        }

        public async Task<List<dynamic>> GetMonthlyInterviewsAsync(Guid applicantId, DateTime startOfYear)
        {
            var startDate = DateOnly.FromDateTime(startOfYear);

            return (await _context.Interviews
                .Where(i =>
                    i.ApplicantId == applicantId &&
                    i.InterviewDate >= startDate
                )
                .GroupBy(i => i.InterviewDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToListAsync())
                .Cast<dynamic>()
                .ToList();
        }

        public async Task<List<RecentApplicationDto>> GetRecentApplicationsAsync(Guid applicantId, int take)
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

        public async Task<List<SavedJobsResponseDto>> GetSavedJobsAsync(Guid applicantId)
        {
            var appliedJobIds = await _context.Applications
                .Where(a => a.ApplicantID == applicantId)
                .Select(a => a.JobPostingID)
                .ToListAsync();

            return await _context.SavedJobs
                .Where(x => x.ApplicantId == applicantId)
                .Select(item => new SavedJobsResponseDto
                {
                    jobId=item.JobPostingId,
                    JobTitle = item.JobPosting.Title,
                    JobDescription = item.JobPosting.Description,
                    JobRequirement = item.JobPosting.Responsibility,
                    TimeAgo=item.JobPosting.PostedDate,

                    CompanyLogoUrl = item.JobPosting.Company.LogoUrl,
                    CompanyLocation = item.JobPosting.Company.Location,
                    CompanyName = item.JobPosting.Company.Name,

                    JobType = item.JobPosting.JobTypes
                        .Select(x => x.ToString())
                        .ToList(),

                    minSalary=item.JobPosting.MinSalary,
                    maxSalary=item.JobPosting.MaxSalary,

                    isApplied = appliedJobIds.Contains(item.JobPostingId)
                })
                .ToListAsync();
        }

        public async Task DeactivateAllResumesAsync(Guid applicantId)
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

        public async Task<string?> GetActiveResumePathAsync(Guid applicantId)
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



        public async Task<List<Applicant>> GetAllApplicantsWithDetailsAsync()
        {
            return await _context.Applicants
                .Include(a => a.User)
                .Include(a => a.ApplicantSkills)
                    .ThenInclude(s => s.Skill)
                .Include(a => a.Applications)
                .Include(a => a.Projects)
                .Include(a => a.Resumes)
                .OrderByDescending(a => a.User.CreatedAt)
                .ToListAsync();
        }

        public async Task<Applicant?> GetApplicantWithAllDetailsAsync(Guid applicantId)
        {
            return await _context.Applicants
                .Include(a => a.User)
                .Include(a => a.ApplicantSkills)
                    .ThenInclude(s => s.Skill)
                .Include(a => a.Applications)
                .Include(a => a.Projects)
                .Include(a => a.Resumes)
                .FirstOrDefaultAsync(a => a.ApplicantID == applicantId);
        }

        public async Task<int> CountBlockedApplicantsAsync()
        {
            return await _context.Applicants.CountAsync(a => a.IsBlocked == true);
        }

        public async Task<int> CountNewApplicantsThisMonthAsync()
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year,DateTime.UtcNow.Month,1);
            return await _context.Applicants
                .CountAsync(a => a.User.CreatedAt >= startOfMonth);
        }
    }
}
