using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface IApplicantSkillService
    {
        Task<SkillResponseDto?> AddSkillAsync(Guid applicantId, SkillDto dto) ;
        Task<bool> DeleteSkillAsync(Guid applicantSkillId, Guid applicantId) ;
        Task<List<SkillResponseDto>> GetAllSkillsAsync(Guid applicantId) ;
        Task<SkillResponseDto?> GetSkillByIdAsync(Guid applicantSkillId, Guid applicantId) ;
    }
}
