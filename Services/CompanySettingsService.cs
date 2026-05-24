using Graduation_Project.Dtos.Company.SettingsTabs;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class CompanySettingsService : ICompanySettingsService
    {
        private readonly ICompanyRepository _companyRepository ;

        public CompanySettingsService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository ;
        }
        public async Task<CompanyProfileSettingsDto?> GetProfileDetailsAsync(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyForSettingsAsync(companyId);
            if(company == null) return null;

            return new CompanyProfileSettingsDto
            {
                CompanyID = company.CompanyID,
                Name = company.Name,
                LogoUrl = company.LogoUrl,
                CoverLogoUrl = company.CoverLogoUrl,
                Industry = company.Industry,
                Country = company.Country,
                CompanySize = company.CompanySize,
                FoundedYear = company.FoundedYear,
                WebsiteURL = company.WebsiteURL,
                ProfileBio = company.ProfileBio,
                Description = company.Description,
                HeadquarterAddress = company.HeadquarterAddress
            };
        }

        public async Task<bool> UpdateProfileAsync(Guid companyId,UpdateCompanyProfileDto dto)
        {
            var company = await _companyRepository.GetCompanyForSettingsAsync(companyId);
            if(company == null) return false;

            company.Name = dto.Name;
            company.LogoUrl = dto.LogoUrl;
            company.CoverLogoUrl = dto.CoverLogoUrl;
            company.Industry = dto.Industry;
            company.Country = dto.Country;
            company.CompanySize = dto.CompanySize;
            company.FoundedYear = dto.FoundedYear;
            company.WebsiteURL = dto.WebsiteURL;
            company.ProfileBio = dto.ProfileBio;
            company.Description = dto.Description;
            company.HeadquarterAddress = dto.HeadquarterAddress;

            return await _companyRepository.UpdateCompanyProfileAsync(company);
        }

        // ====================== Socials Tab ======================

        public async Task<CompanySocialsSettingsDto?> GetSocialsDetailsAsync(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyForSettingsAsync(companyId);
            if(company == null) return null;

            return new CompanySocialsSettingsDto
            {
                PhoneNumber = company.PhoneNumber,
                HeadquarterAddress = company.HeadquarterAddress,
                Country = company.Country,
                Linkedin = company.Linkedin,
                Instagram = company.Instagram,
                Facebook = company.Facebook,
                Twitter = company.Twitter,
                WebsiteURL = company.WebsiteURL
            };
        }

        public async Task<bool> UpdateSocialsAsync(Guid companyId,UpdateCompanySocialsDto dto)
        {
            var company = await _companyRepository.GetCompanyForSettingsAsync(companyId);
            if(company == null) return false;

            company.PhoneNumber = dto.PhoneNumber;
            company.HeadquarterAddress = dto.HeadquarterAddress;
            company.Country = dto.Country;
            company.Linkedin = dto.Linkedin;
            company.Instagram = dto.Instagram;
            company.Facebook = dto.Facebook;
            company.Twitter = dto.Twitter;
            company.WebsiteURL = dto.WebsiteURL;

            return await _companyRepository.UpdateCompanyProfileAsync(company);
        }
    }
}
