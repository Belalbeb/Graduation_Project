using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface IApplicantSkillRepository
    {
        Task<Skill?> GetSkillByNameAsync(string skillName);
        Task<Skill> AddSkillAsync(Skill skill);
        Task<bool> ApplicantSkillExistsAsync(int applicantId, int skillId);
        Task<ApplicantSkill> AddApplicantSkillAsync(ApplicantSkill applicantSkill);
        Task<ApplicantSkill?> GetApplicantSkillAsync(int applicantSkillId, int applicantId);
        Task<List<ApplicantSkill>> GetAllApplicantSkillsAsync(int applicantId);
        Task DeleteApplicantSkillAsync(ApplicantSkill applicantSkill);
    }
}
