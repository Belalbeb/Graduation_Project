using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class ComapnyServices : ICompanyServices
    {
        private readonly ApplicationDbContext dbContext;

        public ComapnyServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Company> AddCompanyAsync(Company company)
        {
           await dbContext.Companies.AddAsync(company);
            await dbContext.SaveChangesAsync();
            return company;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await dbContext.Companies.FindAsync(id);
            if (company == null)
                return false;

          dbContext.Companies.Remove(company);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await dbContext.Companies
               .Include(c => c.User)
               .FirstOrDefaultAsync(c => c.CompanyID == id);
        }

        public async Task<CompanyResponseDto?> GetCompanyDashboardAsync(int companyId)
        {
            // 1. Load company with all needed relations
            var company = await dbContext.Companies
                .Include(c => c.JobPostings)
                    .ThenInclude(jp => jp.Applications)
                        .ThenInclude(a => a.Applicant)
                .Include(c => c.JobPostings)
                    //.ThenInclude(jp => jp.Interviews)
                .FirstOrDefaultAsync(c => c.CompanyID==companyId);

            if (company == null) return null;

            var allJobPostings = company.JobPostings ?? new List<JobPosting>();
            var allApplications = allJobPostings.SelectMany(jp => jp.Applications).ToList();
            var now = DateTime.UtcNow;

            // 2. Statistics
            var statistics = new CompanyResponseDto.StatisticsDto
            {
                TotalJobPosts = allJobPostings.Count(),
                ActiveJobPosts = allJobPostings.Count(jp => jp.IsActive),
                TotalApplicants = allApplications.Count(),
                //ScheduledInterviews = allJobPostings
                //    .SelectMany(jp => jp.Interviews)
                //    .Count(i => i.ScheduledAt >= now)
            };

            // 3. Monthly stats (last 12 months)
            var monthlyStats = Enumerable.Range(0, 12)
                .Select(offset =>
                {
                    var month = now.AddMonths(-offset);
                    var monthStr = month.ToString("MMM");
                    var year = month.Year;
                    var mo = month.Month;

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

            // 4. Recent job posts (latest 10)
            var recentJobPosts = allJobPostings
                .OrderByDescending(jp => jp.PostedDate)
                .Take(10)
                .Select((jp) => new CompanyResponseDto.RecentJobPostsDto
                {
                    Id = jp.JobID,
                    JobTitle = jp.Title,
                    TotalApplication = jp.Applications.Count.ToString(),
                    PostedAt = jp.PostedDate
                })
                .ToList();

            // 5. New applicants (latest 10)
            var applicants = allApplications
                .OrderByDescending(a => a.AppliedDate)
                .Take(10)
                .Select(a => new CompanyResponseDto.ApplicantsDto
                {
                    ApplicantId = a.ApplicantID,
                    ApplicantName = $"{a.Applicant.FirstName} {a.Applicant.LastName}",
                    JobAppliedFor = a.JobPosting.Title,
                    AppliedAt = a.AppliedDate
                })
                .ToList();

            // 6. Assemble and return
            return new CompanyResponseDto
            {
                Statistics = statistics,
                MonthlyStats = monthlyStats,
                RecentJobPosting = recentJobPosts,
                Applicants = applicants
            };
        }

        public async Task<bool> UpdateCompanyAsync(int id, Company updatedCompany)
        {
            var company = await dbContext.Companies.FindAsync(id);
            if (company == null)
                return false;

            company.Name = updatedCompany.Name;
            company.WebsiteURL = updatedCompany.WebsiteURL;
            company.Industry = updatedCompany.Industry;
            company.HeadquarterAddress = updatedCompany.HeadquarterAddress;
         

            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
