using Graduation_Project.Dtos;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class AdminServices : IAdminServices

    {
        private readonly IAdminRepository adminRepository;
        private readonly IUserRepository userRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IJobPostingRepository jobPostingRepository;

        public AdminServices(IAdminRepository adminRepository,IUserRepository userRepository,ICompanyRepository companyRepository,IJobPostingRepository jobPostingRepository)
        {
            this.adminRepository = adminRepository;
            this.userRepository = userRepository;
            this.companyRepository = companyRepository;
            this.jobPostingRepository = jobPostingRepository;
        }
        public async Task<AdminDashboardResponseDto> GetAdminDashboardAsync()
        {
            var userCounts =await userRepository.GetUserCountAsync();
            var CompaniesCount = await companyRepository.GetCompaniesCount();
            var ActiveJobsCount = await jobPostingRepository.GetActiveJobsCountAsync();
            var pendingJobsCount = await jobPostingRepository.GetPendingJobsCountAsync();
            var latestJobs = await jobPostingRepository.GetLatestJobsAsync(5);
            var pendingJobs = await jobPostingRepository.GetPendingApprovalsAsync();
            var monthlyJobStat = await jobPostingRepository.GetMonthlyStatsAsync();

            var MonthlyStats = monthlyJobStat.Select(i => new MonthlyJobStatsDto { Month = i.month, JobPosts = i.JobCount, Applications = i.ApplicationCount });
            AdminDashboardResponseDto adminDashboardResponseDto = new AdminDashboardResponseDto()
            {
                TotalUsers = userCounts,
                TotalCompanies = CompaniesCount,
                ActiveJobPosts = ActiveJobsCount,
                PendingJobs = pendingJobsCount,
                Year= DateTime.UtcNow.Year,
                MonthlyStats = MonthlyStats.ToList(),
                LatestJobs = latestJobs.Select(i => new LatestJobDto
                {
                    JobTitle = i.Title,
                    TotalApplications = i.Applications.Count(),
                    PostedAt = i.PostedDate

                }).ToList(),
                PendingApprovals=pendingJobs.Select(i=>new PendingApprovalDto {JobTitle=i.Title,CreatedAt=i.PostedDate, CompanyName=i.Company.Name,}).ToList()



            };
            return adminDashboardResponseDto;

    
        }
        public async Task<AdminJobOverviewDto> GetAdminJobDashboard()
        {
            var TotalJobs = await jobPostingRepository.TotalJobs();
            var ActiveJobsCount = await jobPostingRepository.GetActiveJobsCountAsync();
            var pendingJobsCount = await jobPostingRepository.GetPendingJobsCountAsync();
            var rejectedJobs = await jobPostingRepository.GetRejectedJobsCount();
            var jobs = await jobPostingRepository.GetAllAsync();
            AdminJobOverviewDto adminJobDashboardDto = new AdminJobOverviewDto
            {
                TotalJobs = TotalJobs,
                ActiveJobs = ActiveJobsCount,
                PendingJobs = pendingJobsCount,
                RejectedJobs = rejectedJobs,
                jobs = jobs.Select(i => new JobView
                {
                    JobId = i.JobID,
                    JobTitle = i.Title,
                    CompanyName = i.Company.Name,
                    CompanyLogo = i.Company.LogoUrl,
                    Category = i.JobCategory,
                    Type = i.JobTypes.Select(x => x.ToString()).ToList(),
                    Status = i.Status.ToString(),
                    Applications = i.Applications.Count(),
                    PostedDate = i.PostedDate


                }).ToList()
            };


            return adminJobDashboardDto;
        }
        public async Task<AdminJobDetailsDto> AdminJobDetails(Guid JobId)
        {
            var job = await jobPostingRepository.GetByIdAsync(JobId);
            if (job == null) return null;
            AdminJobDetailsDto adminJobDetailsDto = new AdminJobDetailsDto
            {
                JobID = job.JobID,
                Title = job.Title,
                Description = job.Description,
                Responsibility = job.Responsibility,
                MaxSalary = job.MaxSalary,
                MinSalary = job.MinSalary,
                JobCategory = job.JobCategory,
                Location = job.Location,
                Status = job.Status.ToString(),
                PostedAgo = GetTimeAgo(job.PostedDate),
                Skills = job.Skills.Select(x => x.ToString()).ToList(),
                JobTypes = job.JobTypes.Select(x => x.ToString()).ToList(),
                WorkApproaches = job.WorkApproaches.Select(x => x.ToString()).ToList(),
                ApplicationsCount = job.Applications.Count(),
                CompanyID = job.CompanyID,
                CompanyName = job.Company.Name,
                CompanyLogoUrl = job.Company.LogoUrl,
                CompanyIndustry = job.Company.Industry,
                MinEmployees = job.Company.MinEmployees,
                MaxEmployees = job.Company.MaxEmployees,
                candidates = job.Applications.Select(x => new CandidateDto
                {
                    ApplicationId = x.ApplicationID,
                    ApplicantId = x.Applicant.ApplicantID,
                    FullName = x.Applicant.FirstName + " " + x.Applicant.LastName,
                    JobTitle = x.Applicant.JobTitle ?? "—",
                    Location = x.Applicant.Location ?? "—",
                    AvatarUrl = x.Applicant.ProfilePicURL,
                    ApplicationStatus = x.ApplicationStatus.ToString(),
                    AppliedAgo = GetTimeAgo(x.AppliedDate),
                    MatchPercentage = 0,   // calculate from your matching logic if available
                    CvUrl = x.Applicant.Resumes
                            .OrderByDescending(r => r.IsActive)
                            .Select(r => r.FilePath)
                            .FirstOrDefault()
                }).ToList()

            };
            return adminJobDetailsDto;

        }
        public async Task<bool> AcceptJobAsync(Guid jobId)
        {
            var result = await jobPostingRepository.AcceptJobAsync(jobId);
            return result;

        }
        public async Task<bool> RejectJobAsync(Guid jobId)
        {
            var result = await jobPostingRepository.RejectJobAsync(jobId);
            return result;

        }
        public static string GetTimeAgo(DateTime date)
        {
            var span = DateTime.UtcNow - date;

            if (span.TotalMinutes < 1)
                return "Just now";

            if (span.TotalHours < 1)
                return $"{(int)span.TotalMinutes} minutes ago";

            if (span.TotalDays < 1)
                return $"{(int)span.TotalHours} hours ago";
            if(span.TotalDays<30)
            return $"{(int)span.TotalDays} days ago";

            return $"{(int)span.TotalDays / 30} month ago";
        }
    }
}
