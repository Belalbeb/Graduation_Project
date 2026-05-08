using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Company> AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            return await _context.Companies
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CompanyID == id);
        }

        public async Task<Company?> GetByUserIdAsync(string userId)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Company?> GetWithJobPostingsAndApplicationsAsync(Guid companyId)
        {
            return await _context.Companies
                .Include(c => c.JobPostings)
                    .ThenInclude(jp => jp.Applications)
                        .ThenInclude(a => a.Applicant)
                .Include(c => c.JobPostings)
                    .ThenInclude(jp => jp.Interviews)
                .FirstOrDefaultAsync(c => c.CompanyID == companyId);
        }

        public async Task<bool> UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return false;

            _context.Companies.Remove(company);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
