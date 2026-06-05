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
        public Task<bool> UpdateResume(Guid applicantId, IFormFile resume,string resumeName);
        public  Task<bool> updatePhoto(Guid applicantId, IFormFile photo);
        public Task<bool> UpdateCoverPhoto(Guid applicantId, IFormFile coverPhoto);
    }
}
