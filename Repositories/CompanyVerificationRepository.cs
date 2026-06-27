using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Graduation_Project.Repositories
{
    public class CompanyVerificationRepository : ICompanyVerificationRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyVerificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CompanyVerificationRequest request)
        {
            await _context.companyVerificationRequests.AddAsync(request);
        }

        public async Task<CompanyVerificationRequest?> GetByIdAsync(Guid id)
        {
            return await _context.companyVerificationRequests
                .Include(x => x.Documents)
                .Include(x=>x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<CompanyVerificationRequest?> GetByCompanyAsync(Guid companyId)
        {
            return await _context.companyVerificationRequests
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.SubmittedAt)
                .FirstOrDefaultAsync();
        }
        public async Task<CompanyVerificationRequest?> GetByIdWithCompanyAsync(Guid id)
        {
            return await _context.companyVerificationRequests
                .Include(x => x.Company)
                .Include(x => x.Documents)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CompanyVerificationRequest>> GetAllRequestAsync()
        {
            return await _context.companyVerificationRequests
                .Include(x => x.Company)
                  .ThenInclude(x=>x.User)
                .Include(x=>x.Documents)
               .OrderByDescending(x => x.SubmittedAt)

                .ToListAsync();
        }

        public async Task UpdateAsync(CompanyVerificationRequest request)
        {
            _context.companyVerificationRequests.Update(request);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
