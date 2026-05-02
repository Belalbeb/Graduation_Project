using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ApplicantSkillService : IApplicantSkillService
    {
        private readonly IApplicantSkillRepository _repository;

        public ApplicantSkillService(IApplicantSkillRepository repository)
        {
            _repository = repository;
        }

        public async Task<SkillResponseDto?> AddSkillAsync(int applicantId, SkillDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SkillName))
                return null;

            var skill = await _repository.GetSkillByNameAsync(dto.SkillName);

            if (skill == null)
            {
                skill = new Skill { SkillName = dto.SkillName.Trim() };
                skill = await _repository.AddSkillAsync(skill);
            }

            var exists = await _repository.ApplicantSkillExistsAsync(applicantId, skill.SkillID);
            if (exists) return null;

            var applicantSkill = new ApplicantSkill
            {
                ApplicantID = applicantId,
                SkillID     = skill.SkillID
            };

            applicantSkill = await _repository.AddApplicantSkillAsync(applicantSkill);

            return new SkillResponseDto
            {
                ApplicantSkillID = applicantSkill.ApplicantSkillID,
                SkillID          = applicantSkill.SkillID,
                SkillName        = skill.SkillName
            };
        }

        public async Task<bool> DeleteSkillAsync(int applicantSkillId, int applicantId)
        {
            var skill = await _repository.GetApplicantSkillAsync(applicantSkillId, applicantId);
            if (skill == null) return false;

            await _repository.DeleteApplicantSkillAsync(skill);
            return true;
        }

        public async Task<List<SkillResponseDto>> GetAllSkillsAsync(int applicantId)
        {
            var skills = await _repository.GetAllApplicantSkillsAsync(applicantId);

            return skills.Select(s => new SkillResponseDto
            {
                ApplicantSkillID = s.ApplicantSkillID,
                SkillID          = s.SkillID,
                SkillName        = s.Skill.SkillName
            }).ToList();
        }

        public async Task<SkillResponseDto?> GetSkillByIdAsync(int applicantSkillId, int applicantId)
        {
            var skills = await _repository.GetAllApplicantSkillsAsync(applicantId);
            var skill = skills.FirstOrDefault(s => s.ApplicantSkillID == applicantSkillId);

            if (skill == null) return null;

            return new SkillResponseDto
            {
                ApplicantSkillID = skill.ApplicantSkillID,
                SkillID          = skill.SkillID,
                SkillName        = skill.Skill.SkillName
            };
        }
    }
}
