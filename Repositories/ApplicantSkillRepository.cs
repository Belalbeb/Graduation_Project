using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class ApplicantSkillRepository : IApplicantSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicantSkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Skill?> GetSkillByNameAsync(string skillName)
        {
            return await _context.Skills
                .FirstOrDefaultAsync(s => s.SkillName.ToLower() == skillName.ToLower().Trim());
        }

        public async Task<Skill> AddSkillAsync(Skill skill)
        {
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        public async Task<bool> ApplicantSkillExistsAsync(int applicantId, int skillId)
        {
            return await _context.ApplicantSkills
                .AnyAsync(s => s.ApplicantID == applicantId && s.SkillID == skillId);
        }

        public async Task<ApplicantSkill> AddApplicantSkillAsync(ApplicantSkill applicantSkill)
        {
            await _context.ApplicantSkills.AddAsync(applicantSkill);
            await _context.SaveChangesAsync();
            return applicantSkill;
        }

        public async Task<ApplicantSkill?> GetApplicantSkillAsync(int applicantSkillId, int applicantId)
        {
            return await _context.ApplicantSkills
                .FirstOrDefaultAsync(s => s.ApplicantSkillID == applicantSkillId && s.ApplicantID == applicantId);
        }

        public async Task<List<ApplicantSkill>> GetAllApplicantSkillsAsync(int applicantId)
        {
            return await _context.ApplicantSkills
                .Where(s => s.ApplicantID == applicantId)
                .Include(s => s.Skill)
                .ToListAsync();
        }

        public async Task DeleteApplicantSkillAsync(ApplicantSkill applicantSkill)
        {
            _context.ApplicantSkills.Remove(applicantSkill);
            await _context.SaveChangesAsync();
        }
    }
}
