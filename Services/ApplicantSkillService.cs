using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class ApplicantSkillService : IApplicantSkillService
    {
        private readonly ApplicationDbContext _context ;
        public ApplicantSkillService(ApplicationDbContext context)
        {
            _context = context ;
        }
        public async Task<SkillResponseDto?> AddSkillAsync(int applicantId,SkillDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.SkillName))
                return null ;

            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.SkillName.ToLower() == dto.SkillName.ToLower().Trim()) ;

            if(skill == null)
            {
                skill = new Skill
                {
                    SkillName = dto.SkillName.Trim()
                } ;
                await _context.Skills.AddAsync(skill);
                await _context.SaveChangesAsync() ;
            }

            var existing = await _context.ApplicantSkills
                .AnyAsync(s => s.ApplicantID == applicantId && s.SkillID == skill.SkillID) ;

            if(existing)
                return null ;

            var applicantSkill = new ApplicantSkill
            {
                ApplicantID = applicantId,
                SkillID = skill.SkillID,
            } ;

            await _context.ApplicantSkills.AddAsync(applicantSkill) ;
            await _context.SaveChangesAsync() ;

            return new SkillResponseDto
            {
                ApplicantSkillID = applicantSkill.ApplicantSkillID,
                SkillID = applicantSkill.SkillID,
                SkillName = skill.SkillName
            } ;
        }

        public async Task<bool> DeleteSkillAsync(int applicantSkillId,int applicantId)
        {
            var skill = await _context.ApplicantSkills
                .FirstOrDefaultAsync(s => s.ApplicantSkillID == applicantSkillId
                && s.ApplicantID == applicantId) ;

            if(skill == null)
                return false ;

            _context.ApplicantSkills.Remove(skill) ;
            await _context.SaveChangesAsync() ;
            return true ;
        }

        public async Task<List<SkillResponseDto>> GetAllSkillsAsync(int applicantId)
        {
            return await _context.ApplicantSkills
                .Where(s => s.ApplicantID == applicantId)
                .Include(s => s.Skill)
                .Select(s => new SkillResponseDto
                {
                    ApplicantSkillID = s.ApplicantSkillID,
                    SkillID = s.SkillID,
                    SkillName = s.Skill.SkillName
                })
                .ToListAsync() ;
        }

        public async Task<SkillResponseDto?> GetSkillByIdAsync(int applicantSkillId,int applicantId)
        {
            return await _context.ApplicantSkills
                .Where(s => s.ApplicantSkillID == applicantSkillId && s.ApplicantID == applicantId)
                .Include(s => s.Skill)
                .Select(s => new SkillResponseDto
                {
                    ApplicantSkillID = s.ApplicantSkillID,
                    SkillID = s.SkillID,
                    SkillName = s.Skill.SkillName
                })
                .FirstOrDefaultAsync() ;
        }
    }
}
