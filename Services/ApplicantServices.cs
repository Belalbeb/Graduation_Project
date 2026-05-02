using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ApplicantServices : IApplicantServices
    {
        private readonly IApplicantRepository _repository;

        public ApplicantServices(IApplicantRepository repository)
        {
            _repository = repository;
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
            return await _repository.CreateAsync(applicant);
        }

        public async Task<bool> DeleteApplicantAsync(int id)
        {
            var applicant = await _repository.GetByIdAsync(id);
            if (applicant == null) return false;

            await _repository.DeleteAsync(applicant);
            return true;
        }

        public async Task<Applicant> GetApplicantByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Applicant>> GetAllApplicantAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ApplicantDashboardResponseDto> GetDashboardAsync(int applicantId)
        {
            var now = DateTime.UtcNow;
            var startOfYear = new DateTime(now.Year, 1, 1);

            var appliedCount       = await _repository.CountApplicationsAsync(applicantId);
            var savedCount         = await _repository.CountSavedJobsAsync(applicantId);
            var upcomingInterviews = await _repository.CountUpcomingInterviewsAsync(applicantId);
            var profileViews       = await _repository.CountProfileViewsAsync(applicantId);

            var applications = await _repository.GetMonthlyApplicationsAsync(applicantId, startOfYear);
            var interviews   = await _repository.GetMonthlyInterviewsAsync(applicantId, startOfYear);

            var monthlyStats = Enumerable.Range(1, 6).Select(m => new MonthlyStatDto
            {
                Month = new DateTime(now.Year, m, 1).ToString("MMM"),
                ApplicationsCount = applications.FirstOrDefault(a => (int)a.Month == m)?.Count ?? 0,
                InterviewsCount   = interviews.FirstOrDefault(i => (int)i.Month == m)?.Count ?? 0
            }).ToList();

            var recentApps = await _repository.GetRecentApplicationsAsync(applicantId, 5);

            return new ApplicantDashboardResponseDto
            {
                Statistics = new StatisticsDto
                {
                    AppliedJobsCount       = appliedCount,
                    SavedJobsCount         = savedCount,
                    UpcomingInterviewsCount = upcomingInterviews,
                    ProfileViewsCount      = profileViews
                },
                MonthlyStats        = monthlyStats,
                RecentApplications  = recentApps
            };
        }

        public async Task<bool> UpdateApplicantAsync(int id, ApplicantDto applicantDto)
        {
            var applicant = await _repository.GetByIdAsync(id);
            if (applicant == null) return false;

            applicant.Location    = applicantDto.Location;
            applicant.FirstName   = applicantDto.FirstName;
            applicant.LastName    = applicantDto.LastName;
            applicant.PhoneNumber = applicantDto.PhoneNumber;

            await _repository.UpdateAsync(applicant);
            return true;
        }

        public async Task<List<SavedJobsResponseDto>> GetSavedsAsync(int id)
        {
            var applicant = await _repository.GetByIdAsync(id);
            if (applicant == null) return null;

            var savedJobs = await _repository.GetSavedJobsAsync(id);

            foreach (var job in savedJobs)
                job.TimeAgo = GetTimeAgo(job.SavedAt);

            return savedJobs;
        }

        public async Task<Resume> UploadResumeAsync(int applicantId, string fileName, string filePath)
        {
            await _repository.DeactivateAllResumesAsync(applicantId);

            var newResume = new Resume
            {
                FileName   = fileName,
                FilePath   = filePath,
                UploadDate = DateTime.UtcNow,
                IsActive   = true,
                ApplicantID = applicantId
            };

            await _repository.AddResumeAsync(newResume);
            return newResume;
        }

        public async Task<string?> GetActiveResumePathAsync(int applicantId)
        {
            return await _repository.GetActiveResumePathAsync(applicantId);
        }

        public async Task<string?> GetActiveResumePathByUserIdAsync(string userId)
        {
            return await _repository.GetActiveResumePathByUserIdAsync(userId);
        }

        public async Task<Applicant> GetApplicantByUserIdAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }
    }
}
