using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface ISettingsService
    {
        // Profile Tab
        Task<SettingsProfileDto?> GetProfileDetailsAsync(Guid applicantId) ;
        Task<bool> UpdateProfileAsync(Guid applicantId,UpdateProfileDto dto) ;

        // Contact Tab
        Task<SettingsContactDto> GetContactDetailsAsync(Guid applicantId) ;
        Task<bool> UpdateContactAsync(Guid applicantId,UpdateContactDto dto) ;
    }
}
