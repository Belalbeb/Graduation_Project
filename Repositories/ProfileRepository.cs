using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Applicant?> GetApplicantByUserIdAsync(string userId)
        {
            return await _context.Applicants.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Company?> GetCompanyByUserIdAsync(string userId)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Applicant?> GetPublicProfileAsync(Guid applicantId)
        {
            return await _context.Applicants
                .Include(a => a.Experiences)
                .Include(u=>u.User)
                .Include(a => a.ApplicantSkills)
                    .ThenInclude(s => s.Skill)
                .Include(a => a.Projects)
                .Include(a=>a.Resumes)
                .FirstOrDefaultAsync(a => a.ApplicantID == applicantId);
        }
    }
}
