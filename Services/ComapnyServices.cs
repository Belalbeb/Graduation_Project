using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ComapnyServices : ICompanyServices
    {
        private readonly ICompanyRepository _repository;

        public ComapnyServices(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            return await _repository.AddAsync(company);
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Company?> GetCompanyByUserIdAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task<CompanyResponseDto?> GetCompanyDashboardAsync(int companyId)
        {
            var company = await _repository.GetWithJobPostingsAndApplicationsAsync(companyId);
            if (company == null) return null;

            var allJobPostings = company.JobPostings ?? new List<JobPosting>();
            var allApplications = allJobPostings.SelectMany(jp => jp.Applications).ToList();
            var now = DateTime.UtcNow;

            var statistics = new CompanyResponseDto.StatisticsDto
            {
                TotalJobPosts       = allJobPostings.Count(),
                ActiveJobPosts      = allJobPostings.Count(jp => jp.IsActive),
                TotalApplicants     = allApplications.Count(),
                ScheduledInterviews = allJobPostings
                    .SelectMany(jp => jp.Interviews)
                    .Count(i => i.ScheduledAt >= now)
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
                    ApplicantName = $"{a.Applicant.FirstName} {a.Applicant.LastName}",
                    JobAppliedFor = a.JobPosting.Title,
                    AppliedAt     = a.AppliedDate
                })
                .ToList();

            return new CompanyResponseDto
            {
                Statistics       = statistics,
                MonthlyStats     = monthlyStats,
                RecentJobPosting = recentJobPosts,
                Applicants       = applicants
            };
        }

        public async Task<bool> UpdateCompanyAsync(int id, Company updatedCompany)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null) return false;

            company.Name               = updatedCompany.Name;
            company.WebsiteURL         = updatedCompany.WebsiteURL;
            company.Industry           = updatedCompany.Industry;
            company.HeadquarterAddress = updatedCompany.HeadquarterAddress;

            return await _repository.UpdateAsync(company);
        }
    }
}
