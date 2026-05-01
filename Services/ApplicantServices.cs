using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Graduation_Project.Services
{
    public class ApplicantServices : IApplicantServices
    {
        private readonly ApplicationDbContext dbContext;

        public ApplicantServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public static string GetTimeAgo(DateTime dateTime)
        {
            var diff = DateTime.UtcNow - dateTime;

            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} minutes ago";

            if (diff.TotalHours < 24)
                return $"{(int)diff.TotalHours} hours ago";

            if (diff.TotalDays < 7)
                return $"{(int)diff.TotalDays} days ago";

            if (diff.TotalDays < 30)
                return $"{(int)(diff.TotalDays / 7)} weeks ago";

            return $"{(int)(diff.TotalDays / 30)} months ago";
        }
        public async Task<Applicant> CreateApplicantAsync(Applicant applicant)
        {
          await dbContext.Applicants.AddAsync(applicant);
            await dbContext.SaveChangesAsync();
            return applicant;
        }

        public async Task<bool> DeleteApplicantAsync(int id)
        {
            var applicant = await this.GetApplicantByIdAsync(id);
            if (applicant == null)
                return false;
            else
            {
                dbContext.Applicants.Remove(applicant);
              await dbContext.SaveChangesAsync();
                return true;
            }
        }
          public async Task<Applicant> GetApplicantByIdAsync(int id)
        {
          var applicant=await dbContext.Applicants.FirstOrDefaultAsync(x => x.ApplicantID == id);
            return (applicant==null)? null:applicant;
        }

        public async Task<List<Applicant>> GetAllApplicantAsync()
        {
            return await dbContext.Applicants.ToListAsync();
        }

        public async Task<ApplicantDashboardResponseDto> GetDashboardAsync(int applicantId)
        {
            var now = DateTime.UtcNow;
            var startOfYear = new DateTime(now.Year, 1, 1);

        
            var appliedCount = await dbContext.Applications
                .CountAsync(a => a.ApplicantID == applicantId);

            var savedCount = await dbContext.SavedJobs
                .CountAsync(s => s.ApplicantId == applicantId);

            var upcomingInterviews = await dbContext.Interviews
                .CountAsync(i => i.ApplicantId == applicantId && i.ScheduledAt >= now);

            var profileViews = await dbContext.ProfileViews
                .CountAsync(p => p.ApplicantId == applicantId);

         
            var applications = await dbContext.Applications
                .Where(a => a.ApplicantID == applicantId && a.AppliedDate >= startOfYear)
                .GroupBy(a => a.AppliedDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var interviews = await dbContext.Interviews
                .Where(i => i.ApplicantId == applicantId && i.ScheduledAt >= startOfYear)
                .GroupBy(i => i.ScheduledAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var monthlyStats = Enumerable.Range(1, 6).Select(m => new MonthlyStatDto
            {
                Month = new DateTime(now.Year, m, 1).ToString("MMM"),
                ApplicationsCount = applications.FirstOrDefault(a => a.Month == m)?.Count ?? 0,
                InterviewsCount = interviews.FirstOrDefault(i => i.Month == m)?.Count ?? 0
            }).ToList();

            var recentApps = await dbContext.Applications
                .Where(a => a.ApplicantID == applicantId)
                .OrderByDescending(a => a.AppliedDate)
                .Take(5)
                .Select(a => new RecentApplicationDto
                {
                    Id = a.ApplicationID,
                    CompanyName = a.JobPosting.Company.Name,
                    CompanyLogoUrl = a.JobPosting.Company.LogoUrl,
                    JobTitle = a.JobPosting.Title,
                    AppliedAt = a.AppliedDate
                })
                .ToListAsync();

            return new ApplicantDashboardResponseDto
            {
                Statistics = new StatisticsDto
                {
                    AppliedJobsCount = appliedCount,
                    SavedJobsCount = savedCount,
                    UpcomingInterviewsCount = upcomingInterviews,
                    ProfileViewsCount = profileViews
                },
                MonthlyStats = monthlyStats,
                RecentApplications = recentApps
            };
        }

        public async Task<bool> UpdateApplicantAsync(int id, ApplicantDto applicant)
        {
            var foundedApplicant =await this.GetApplicantByIdAsync(id);
            if (foundedApplicant == null)
                return false;
            foundedApplicant.Location = applicant.Location;
            foundedApplicant.FirstName = applicant.FirstName;
            foundedApplicant.LastName = applicant.LastName;
            foundedApplicant.PhoneNumber = applicant.PhoneNumber;
            dbContext.Applicants.Update(foundedApplicant);
           await  dbContext.SaveChangesAsync();
            return true;

        }
        public async Task<List<SavedJobsResponseDto>> GetSavedsAsync(int id)
        {
            var applicant =await GetApplicantByIdAsync(id);
            if (applicant == null) return null;
            var savedJobs = await dbContext.SavedJobs
       .Where(x => x.ApplicantId == id)
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

            foreach (var job in savedJobs)
            {
                job.TimeAgo = GetTimeAgo(job.SavedAt);
            }
            return savedJobs;
        }

        public async Task<Resume> UploadResumeAsync(int applicantId,string fileName,string filePath)
        {
            await dbContext.Resumes.Where(r => r.ApplicantID == applicantId)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(r => r.IsActive,false));

            var newResume = new Resume
            {
                FileName = fileName,
                FilePath = filePath,
                UploadDate = DateTime.UtcNow,
                IsActive = true,
                ApplicantID = applicantId
            };

            await dbContext.Resumes.AddAsync(newResume);
            await dbContext.SaveChangesAsync();
            return newResume;
        }
        public async Task<string?> GetActiveResumePathAsync(int applicantId)
        {
            return await dbContext.Resumes
                .Where(r => r.ApplicantID == applicantId && r.IsActive)
                .Select(r => r.FilePath)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetActiveResumePathByUserIdAsync(string userId)
        {
            return await dbContext.Applicants
                .Where(a => a.UserId == userId)
                .SelectMany(a => a.Resumes)
                .Where(r => r.IsActive)
                .Select(r => r.FilePath)
                .FirstOrDefaultAsync();
        }
    }
}
