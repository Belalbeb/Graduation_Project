using Graduation_Project.Dtos.Company.SettingsTabs;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class CompanySettingsService : ICompanySettingsService
    {
        private readonly ICompanyRepository _companyRepository ;
        private readonly CloudinaryService cloudinaryService;

        public CompanySettingsService(ICompanyRepository companyRepository,CloudinaryService cloudinaryService)
        {
            _companyRepository = companyRepository ;
            this.cloudinaryService = cloudinaryService;
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
             
                ProfileBio = company.ProfileBio,
                Description = company.Description,
               
            };
        }

        public async Task<bool> UpdateProfileAsync(Guid companyId,UpdateCompanyProfileDto dto)
        {
            var company = await _companyRepository.GetCompanyForSettingsAsync(companyId);
            if(company == null) return false;

            company.Name = dto.Name;
          
            company.Industry = dto.Industry;
            company.Country = dto.Country;
            company.CompanySize = dto.CompanySize;
            company.FoundedYear = dto.FoundedYear;
       
            company.ProfileBio = dto.ProfileBio;
            company.Description = dto.Description;
         

            return await _companyRepository.UpdateCompanyProfileAsync(company);
        }
        public async Task<bool> UpdateCoverImage(Guid CompanyId,IFormFile cover)
        {
            var company = await _companyRepository.GetCompanyForSettingsAsync(CompanyId);
            if (company == null) return false;
            var coverLink = await cloudinaryService.UploadImageAsync(cover);
            if (coverLink == null) return false;
            company.CoverLogoUrl = coverLink;
            return await _companyRepository.UpdateCompanyProfileAsync(company);
           

        }
        public async Task<bool> UpdateLogo(Guid CompanyId, IFormFile logo)
        {
            var company = await _companyRepository.GetCompanyForSettingsAsync(CompanyId);
            if (company == null) return false;
            var logoLink = await cloudinaryService.UploadImageAsync(logo);
            if (logoLink == null) return false;
            company.LogoUrl = logoLink;
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
       
            company.Linkedin = dto.Linkedin;
            company.Instagram = dto.Instagram;
            company.Facebook = dto.Facebook;
            company.Twitter = dto.Twitter;
            company.WebsiteURL = dto.WebsiteURL;

            return await _companyRepository.UpdateCompanyProfileAsync(company);
        }
    }
}
