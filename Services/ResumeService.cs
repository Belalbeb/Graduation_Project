using System.Security.AccessControl;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class ResumeService : IResumeServices
    {
        private readonly ApplicationDbContext _context ;

        public ResumeService(ApplicationDbContext context)
        {
            _context = context ;
        }

        public async Task<Resume> UploadNewResumeAsync(int applicantId,string fileName,string filePath)
        {
            await _context.Resumes.Where(r => r.ApplicantID == applicantId)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(r => r.IsActive, false)) ;

            var newResume = new Resume
            {
                FileName = fileName,
                FilePath = filePath,
                UploadDate = DateTime.UtcNow,
                IsActive = true,
                ApplicantID = applicantId
            } ;

            await _context.Resumes.AddAsync(newResume) ;
            await _context.SaveChangesAsync() ;
            return newResume ;
        }
        public async Task<string?> GetActiveResumeAsync(int applicantId)
        {
            return await _context.Resumes
                .Where(r => r.ApplicantID == applicantId && r.IsActive)
                .Select(r => r.FilePath)
                .FirstOrDefaultAsync() ;
        }

        public async Task<string?> GetActiveResumePathByUserIdAsync(string userId)
        {
            return await _context.Applicants
                .Where(a => a.UserId == userId)
                .SelectMany(a => a.Resumes)
                .Where(r => r.IsActive)
                .Select(r => r.FilePath)
                .FirstOrDefaultAsync() ;
        }
    }
}
