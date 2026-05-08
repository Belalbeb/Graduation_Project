using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly ApplicationDbContext _context;

        public ExperienceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Experience?> GetByIdAsync(Guid experienceId)
        {
            return await _context.Experiences.FirstOrDefaultAsync(e => e.ExperienceID == experienceId);
        }

        public async Task<List<Experience>> GetAllByApplicantAsync(Guid applicantId)
        {
            return await _context.Experiences
                .Where(e => e.ApplicantID == applicantId)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingExperienceAsync(Experience experience)
        {
            return await _context.Experiences
                .AnyAsync(e => e.ApplicantID == experience.ApplicantID &&
                               e.ExperienceID != experience.ExperienceID &&
                               e.StartDate < experience.EndDate &&
                               e.EndDate > experience.StartDate);
        }

        public async Task<Experience> AddAsync(Experience experience)
        {
            await _context.Experiences.AddAsync(experience);
            await _context.SaveChangesAsync();
            return experience;
        }

        public async Task UpdateAsync(Experience experience)
        {
            _context.Experiences.Update(experience);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Experience experience)
        {
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
        }
    }
}
