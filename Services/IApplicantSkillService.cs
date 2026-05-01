using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface IApplicantSkillService
    {
        Task<SkillResponseDto?> AddSkillAsync(int applicantId, SkillDto dto) ;
        Task<bool> DeleteSkillAsync(int applicantSkillId, int applicantId) ;
        Task<List<SkillResponseDto>> GetAllSkillsAsync(int applicantId) ;
        Task<SkillResponseDto?> GetSkillByIdAsync(int applicantSkillId, int applicantId) ;
    }
}
