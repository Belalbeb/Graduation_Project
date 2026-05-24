using Graduation_Project.Dtos.Company.SettingsTabs;

namespace Graduation_Project.Services
{
    public interface ICompanySettingsService
    {
        Task<CompanyProfileSettingsDto?> GetProfileDetailsAsync(Guid companyId);
        Task<bool> UpdateProfileAsync(Guid companyId,UpdateCompanyProfileDto dto);

        // Socials Tab
        Task<CompanySocialsSettingsDto?> GetSocialsDetailsAsync(Guid companyId);
        Task<bool> UpdateSocialsAsync(Guid companyId,UpdateCompanySocialsDto dto);
    }
}
