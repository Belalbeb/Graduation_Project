using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IApplicantSkillRepository
    {
        Task<Skill?> GetSkillByNameAsync(string skillName);
        Task<Skill> AddSkillAsync(Skill skill);
        Task<bool> ApplicantSkillExistsAsync(Guid applicantId, Guid skillId);
        Task<ApplicantSkill> AddApplicantSkillAsync(ApplicantSkill applicantSkill);
        Task<ApplicantSkill?> GetApplicantSkillAsync(Guid applicantSkillId, Guid applicantId);
        Task<List<ApplicantSkill>> GetAllApplicantSkillsAsync(Guid applicantId);
        Task DeleteApplicantSkillAsync(ApplicantSkill applicantSkill);
    }
}
