using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Applicant?> GetApplicantByIdAsync(Guid applicantId)
        {
            return await _context.Applicants.Include(x=>x.User).FirstOrDefaultAsync(x=>x.ApplicantID==applicantId);
        }

        public async Task<Resume?> GetActiveResumeAsync(Guid applicantId)
        {
            return await _context.Resumes
                .FirstOrDefaultAsync(r => r.ApplicantID == applicantId && r.IsActive);
        }

        public async Task UpdateApplicantAsync(Applicant applicant)
        {
            _context.Applicants.Update(applicant);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAllResumesAsync(Guid applicantId)
        {
            await _context.Resumes
                .Where(r => r.ApplicantID == applicantId)
                .ExecuteUpdateAsync(s => s.SetProperty(r => r.IsActive, false));
        }

        public async Task AddResumeAsync(Resume resume)
        {
            await _context.Resumes.AddAsync(resume);
            await _context.SaveChangesAsync();
        }
    }
}
