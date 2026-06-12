using Graduation_Project.Dtos;
using Graduation_Project.Dtos.Admin.Applicant;
using Graduation_Project.Dtos.Admin.Company;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class AdminServices : IAdminServices

    {
        private readonly IAdminRepository adminRepository;
        private readonly IUserRepository userRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IJobPostingRepository jobPostingRepository;
        private readonly IApplicantRepository _applicantRepository ;

        public AdminServices(IAdminRepository adminRepository,IUserRepository userRepository,ICompanyRepository companyRepository,IJobPostingRepository jobPostingRepository,
            IApplicantRepository applicantRepository)
        {
            this.adminRepository = adminRepository;
            this.userRepository = userRepository;
            this.companyRepository = companyRepository;
            this.jobPostingRepository = jobPostingRepository;
            this._applicantRepository = applicantRepository ;
        }

        #region Belal
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
        #endregion

        // ======================= Applicants =============================
        #region Applicants
        public async Task<AdminUserStatsDto> GetUserStatsAsync()
        {
            return new AdminUserStatsDto
            {
                TotalUsers = await userRepository.GetTotalUsersCountAsync(),
                NewUsersThisMonth = await _applicantRepository.CountNewApplicantsThisMonthAsync(),
                BlockedUsers = await _applicantRepository.CountBlockedApplicantsAsync()
            };
        }

        public async Task<List<AdminUserListDto>> GetAllApplicantsAsync()
        {
            var applicants = await _applicantRepository.GetAllApplicantsWithDetailsAsync();

            return applicants.Select(a => new AdminUserListDto
            {
                ApplicantId = a.ApplicantID,
                FullName = $"{a.FirstName} {a.LastName}",
                Email = a.User?.Email ?? a.Email ?? string.Empty,
                JobTitle = a.JobTitle,
                Status = a.IsBlocked ? "Blocked" : a.Status.ToString(),
                Location = a.Location,
                JoinedDate = a.User?.CreatedAt ?? DateTime.UtcNow
            }).ToList();
        }

        public async Task<AdminUserDetailsDto?> GetApplicantDetailsAsync(Guid applicantId)
        {
            var applicant = await _applicantRepository.GetApplicantWithAllDetailsAsync(applicantId);
            if(applicant == null) return null;

            var activeResume = applicant.Resumes?.FirstOrDefault(r => r.IsActive);

            return new AdminUserDetailsDto
            {
                // Basic Info
                ApplicantId = applicant.ApplicantID,
                FullName = $"{applicant.FirstName} {applicant.LastName}",
                JobTitle = applicant.JobTitle,
                Status = applicant.IsBlocked ? "Blocked" : applicant.Status.ToString(),
                JoinedDate = applicant.User?.CreatedAt ?? DateTime.UtcNow,

                // Contact Info
                Email = applicant.User?.Email ?? applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                Location = applicant.Location,

                // Statistics
                ApplicationsCount = applicant.Applications?.Count ?? 0,
                SavedJobsCount = await _applicantRepository.CountSavedJobsAsync(applicantId),
                InterviewsCount = await _applicantRepository.CountUpcomingInterviewsAsync(applicantId),
                ProjectsCount = applicant.Projects?.Count ?? 0,

                // Skills
                Skills = applicant.ApplicantSkills?.Select(s => s.Skill.SkillName).ToList() ?? new List<string>(),

                // Social Links & CV
                CvUrl = activeResume?.FilePath,
                Portfolio = applicant.Portfolio,
                Facebook = applicant.Facebook,
                Linkedin = applicant.Linkedin,
                Github = applicant.Github
            };
        }

        public async Task<bool> BlockApplicantAsync(Guid applicantId)
        {
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if(applicant == null) return false;

            applicant.IsBlocked = true;
            applicant.Status = UserStatus.Blocked;
            await _applicantRepository.UpdateAsync(applicant);
            return true;
        }

        public async Task<bool> UnblockApplicantAsync(Guid applicantId)
        {
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if(applicant == null) return false;

            applicant.IsBlocked = false;
            applicant.Status = UserStatus.Active;
            await _applicantRepository.UpdateAsync(applicant);
            return true;
        }

        public async Task<bool> ApproveApplicantAsync(Guid applicantId)
        {
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if(applicant == null) return false;

            applicant.Status = UserStatus.Active;
            await _applicantRepository.UpdateAsync(applicant);
            return true;
        }

        public async Task<bool> DeleteApplicantAsync(Guid applicantId)
        {
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if(applicant == null) return false;

            await _applicantRepository.DeleteAsync(applicant);
            return true;
        }

        #endregion

        // ======================= Company ===========================
        #region Company
        // Get company statistics for admin dashboard
        public async Task<AdminCompanyStatsDto> GetCompanyStatsAsync()
        {
            return new AdminCompanyStatsDto
            {
                TotalCompanies = await companyRepository.CountTotalCompaniesAsync(),
                VerifiedCompanies = await companyRepository.CountVerifiedCompaniesAsync(),
                VerificationRequests = await companyRepository.CountPendingCompaniesAsync()
            };
        }

        // Get all companies for the companies list page
        public async Task<List<AdminCompanyListDto>> GetAllCompaniesAsync()
        {
            var companies = await companyRepository.GetAllCompaniesForAdminAsync();
            var result = new List<AdminCompanyListDto>();

            foreach(var company in companies)
            {
                var totalJobs = await companyRepository.CountCompanyJobsAsync(company.CompanyID);
                var subscriptionPlan = await companyRepository.GetCompanyActiveSubscriptionPlanAsync(company.CompanyID);

                result.Add(new AdminCompanyListDto
                {
                    CompanyId = company.CompanyID,
                    Name = company.Name,
                    Location = company.Location,
                    Country = company.Country,
                    Email = company.User?.Email ?? string.Empty,
                    Industry = company.Industry,
                    TotalJobs = totalJobs,
                    Status = company.IsBlocked ? "Blocked" : company.Status.ToString(),
                    JoinedDate = company.User?.CreatedAt ?? DateTime.UtcNow,
                    SubscriptionPlan = subscriptionPlan ?? "Free"
                });
            }

            return result;
        }

        // Get detailed information for a single company
        public async Task<AdminCompanyDetailsDto?> GetCompanyDetailsAsync(Guid companyId)
        {
            var company = await companyRepository.GetCompanyWithDetailsForAdminAsync(companyId);
            if(company == null) return null;

            var totalJobs = await companyRepository.CountCompanyJobsAsync(companyId);
            var activeJobs = await companyRepository.CountCompanyActiveJobsAsync(companyId);
            var totalApplicants = await companyRepository.CountCompanyApplicantsAsync(companyId);
            var totalInterviews = await companyRepository.CountCompanyInterviewsAsync(companyId);
            var subscriptionPlan = await companyRepository.GetCompanyActiveSubscriptionPlanAsync(companyId);

            return new AdminCompanyDetailsDto
            {
                // Basic Info
                CompanyId = company.CompanyID,
                Name = company.Name,
                Description = company.Description,
                LogoUrl = company.LogoUrl,
                CoverLogoUrl = company.CoverLogoUrl,

                // Contact & Info
                Email = company.User?.Email ?? string.Empty,
                Industry = company.Industry,
                Location = company.Location,
                Country = company.Country,
                CompanySize = company.CompanySize,

                // Status
                Status = company.IsBlocked ? "Blocked" : company.Status.ToString(),
                IsBlocked = company.IsBlocked,

                // Subscription
                SubscriptionPlan = subscriptionPlan ?? "Free",

                // Statistics
                TotalJobs = totalJobs,
                ActiveJobs = activeJobs,
                TotalApplicants = totalApplicants,
                TotalInterviews = totalInterviews
            };
        }

        // Verify/Approve a company (change status from Pending to Verified)
        public async Task<bool> VerifyCompanyAsync(Guid companyId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if(company == null) return false;

            company.Status = CompanyStatus.Verified;
            await companyRepository.UpdateAsync(company);
            return true;
        }

        // Block a company (prevents posting new jobs)
        public async Task<bool> BlockCompanyAsync(Guid companyId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if(company == null) return false;

            company.IsBlocked = true;
            company.Status = CompanyStatus.Blocked;
            await companyRepository.UpdateAsync(company);
            return true;
        }

        // Unblock a company
        public async Task<bool> UnblockCompanyAsync(Guid companyId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if(company == null) return false;

            company.IsBlocked = false;
            company.Status = CompanyStatus.Verified;
            await companyRepository.UpdateAsync(company);
            return true;
        }

        // Delete a company permanently
        public async Task<bool> DeleteCompanyAsync(Guid companyId)
        {
            var company = await companyRepository.GetByIdAsync(companyId);
            if(company == null) return false;

            await companyRepository.DeleteAsync(companyId);
            return true;
        }
        #endregion
    }
}
