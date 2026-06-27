using Graduation_Project.Dtos;
using Graduation_Project.Dtos.Company.Profile;
using Graduation_Project.Exceptions;
using Graduation_Project.Models;
using Graduation_Project.Repositories;
using Newtonsoft.Json.Serialization;

namespace Graduation_Project.Services
{
    public class ComapnyServices : ICompanyServices
    {
        private readonly ICompanyRepository _repository;
        private readonly IApplicantRepository applicantRepository;

        public ComapnyServices(ICompanyRepository repository,IApplicantRepository applicantRepository)
        {
            _repository = repository;
            this.applicantRepository = applicantRepository;
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            return await _repository.AddAsync(company);
        }

        public async Task<bool> DeleteCompanyAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<Company?> GetCompanyByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Company?> GetCompanyByUserIdAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task<CompanyResponseDto?> GetCompanyDashboardAsync(Guid companyId)
        {
            var company = await _repository.GetWithJobPostingsAndApplicationsAsync(companyId);
            if (company == null) return null;

            var allJobPostings = company.JobPostings ?? new List<JobPosting>();
            var allApplications = allJobPostings.SelectMany(jp => jp.Applications).ToList();
            var now = DateTime.UtcNow;

            var statistics = new CompanyResponseDto.StatisticsDto
            {
                TotalJobPosts       = allJobPostings.Count(),
                ActiveJobPosts      = allJobPostings.Count(jp=>jp.IsActive&&jp.Status==JobStatus.Approved),
                TotalApplicants     = allApplications.Count(),
                ScheduledInterviews = allJobPostings
                    .SelectMany(jp => jp.Interviews)
                    .Count(i => i.InterviewDate >= DateOnly.FromDateTime(now))
            };

            var monthlyStats = Enumerable.Range(0, 12)
                .Select(offset =>
                {
                    var month   = now.AddMonths(-offset);
                    var monthStr = month.ToString("MMM");
                    var year    = month.Year;
                    var mo      = month.Month;

                    return new CompanyResponseDto.MonthlyStatDto
                    {
                        Month = monthStr,
                        ApplicantsCount = allApplications.Count(a =>
                            a.AppliedDate.Year == year && a.AppliedDate.Month == mo),
                        JobPostedCount = allJobPostings.Count(jp =>
                            jp.PostedDate.Year == year && jp.PostedDate.Month == mo)
                    };
                })
                .OrderBy(m =>
                {
                    var parsed = DateTime.ParseExact(m.Month, "MMM",
                        System.Globalization.CultureInfo.InvariantCulture);
                    return parsed.Month;
                })
                .ToList();

            var recentJobPosts = allJobPostings
                .OrderByDescending(jp => jp.PostedDate)
                .Take(10)
                .Select(jp => new CompanyResponseDto.RecentJobPostsDto
                {
                    Id               = jp.JobID,
                    JobTitle         = jp.Title,
                    TotalApplication = jp.Applications.Count.ToString(),
                    PostedAt         = jp.PostedDate
                })
                .ToList();

            var applicants = allApplications
                .OrderByDescending(a => a.AppliedDate)
                .Take(10)
                .Select(a => new CompanyResponseDto.ApplicantsDto
                {
                    ApplicantId   = a.ApplicantID,
                    ImageUrl=a.Applicant.ProfilePicURL,

                    ApplicantName = $"{a.Applicant.FirstName} {a.Applicant.LastName}",
                    JobId=a.JobPostingID,
                    JobAppliedFor = a.JobPosting.Title,
                    AppliedAt     = a.AppliedDate
                })
                .ToList();

            return new CompanyResponseDto
            {
                CompanyName=company.Name,
                Statistics       = statistics,
                MonthlyStats     = monthlyStats,
                RecentJobPosting = recentJobPosts,
                Applicants       = applicants
            };
        }

        public async Task<bool> UpdateCompanyAsync(Guid id, Company updatedCompany)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null) return false;

            company.Name               = updatedCompany.Name;
            company.WebsiteURL         = updatedCompany.WebsiteURL;
            company.Industry           = updatedCompany.Industry;
            company.HeadquarterAddress = updatedCompany.HeadquarterAddress;

            return await _repository.UpdateAsync(company);
        }

        public async Task<CompanyPublicProfileDto?> GetCompanyProfileAsync(Guid companyId)
        {
            var company = await _repository.GetByIdAsync(companyId);
            if(company == null) return null;

            var jobs = await _repository.GetJobPostingsByCompanyIdAsync(companyId);

            var activeJobs = jobs.Where(j => j.IsActive && j.Status == JobStatus.Approved).ToList();
            var totalJobs = jobs.Count;

            return new CompanyPublicProfileDto
            {
                CompanyId = company.CompanyID,
                Name = company.Name,
                IsVerified = company.Status == CompanyStatus.Verified,
                LogoUrl = company.LogoUrl,
                WebsiteUrl=company.WebsiteURL,
                CoverLogoUrl = company.CoverLogoUrl,
                Tagline = company.ProfileBio,
                About = company.Description,
                Address = company.HeadquarterAddress,
                Country = company.Country,
                Industry = company.Industry,
                CompanySize = company.CompanySize,
                FoundedYear = company.FoundedYear,
                Phone=company.PhoneNumber,

                SocialLinks = new CompanySocialLinksDto
                {
                    Facebook = company.Facebook,
                    Linkedin = company.Linkedin,
                    Instagram = company.Instagram,
                    Twitter = company.Twitter
                },

                Stats = new CompanyStatsDto
                {
                    TotalJobs = totalJobs,
                    ActiveJobs = activeJobs.Count
                },

                OpenVacancies = activeJobs.Select(job => new CompanyVacancyDto
                {
                    JobId = job.JobID,
                    Title = job.Title,
                    Description = job.Description,
                    MinSalary = job.MinSalary,
                    MaxSalary = job.MaxSalary,
                    SalaryCurrency = "USD",
                    JobType = job.JobTypes.FirstOrDefault().ToString(),
                    WorkApproach = job.WorkApproaches.FirstOrDefault().ToString(),
                    PostedAt = job.PostedDate
                }).ToList()
            };
        }
        public async Task<CompanyCandidateDto> GetAllCandidate(int page, CandidateFilterDto filter)
        {
            var result = await applicantRepository.GetCandidatesAsync(page, filter);

            var candidatesDto = result.Items.Select(x => new CandidateDetailsDto
            {
                Id = x.ApplicantID,
                Name = $"{x.FirstName} {x.LastName}",
                Image = x.ProfilePicURL,
                Industry = x.Industry,
                JobTitle = x.JobTitle,
                Country = x.Location
            }).ToList();
            //var totalCandidates = await applicantRepository.CountActiveApplicant();
            return new CompanyCandidateDto
            {
                candidates = candidatesDto,
                totalCandidates = result.TotalCount
            };
        }
    }
}
