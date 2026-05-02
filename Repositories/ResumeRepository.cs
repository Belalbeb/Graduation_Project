using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ApplicationDbContext _context;

        public ResumeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeactivateAllAsync(int applicantId)
        {
            await _context.Resumes
                .Where(r => r.ApplicantID == applicantId)
                .ExecuteUpdateAsync(s => s.SetProperty(r => r.IsActive, false));
        }

        public async Task<Resume> AddAsync(Resume resume)
        {
            await _context.Resumes.AddAsync(resume);
            await _context.SaveChangesAsync();
            return resume;
        }

        public async Task<string?> GetActivePathAsync(int applicantId)
        {
            return await _context.Resumes
                .Where(r => r.ApplicantID == applicantId && r.IsActive)
                .Select(r => r.FilePath)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetActivePathByUserIdAsync(string userId)
        {
            return await _context.Applicants
                .Where(a => a.UserId == userId)
                .SelectMany(a => a.Resumes)
                .Where(r => r.IsActive)
                .Select(r => r.FilePath)
                .FirstOrDefaultAsync();
        }
    }
}
