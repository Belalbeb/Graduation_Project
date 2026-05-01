using Graduation_Project.Dtos;

namespace Graduation_Project.Services
{
    public interface ISettingsService
    {
        // Profile Tab
        Task<SettingsProfileDto?> GetProfileDetailsAsync(int applicantId) ;
        Task<bool> UpdateProfileAsync(int applicantId,UpdateProfileDto dto) ;

        // Contact Tab
        Task<SettingsContactDto> GetContactDetailsAsync(int applicantId) ;
        Task<bool> UpdateContactAsync(int applicantId,UpdateContactDto dto) ;
    }
}
