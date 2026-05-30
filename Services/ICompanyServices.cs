using Graduation_Project.Dtos;
using Graduation_Project.Dtos.Company.Profile;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface ICompanyServices
    {
        Task<Company> AddCompanyAsync(Company company);
        Task<Company?> GetCompanyByIdAsync(Guid id);
      
        Task<bool> UpdateCompanyAsync(Guid id, Company updatedCompany);
        Task<bool> DeleteCompanyAsync(Guid id);
        public Task<CompanyResponseDto?> GetCompanyDashboardAsync(Guid CompanyId);
        public Task<Company?> GetCompanyByUserIdAsync(string userid);
        Task<CompanyPublicProfileDto?> GetCompanyProfileAsync(Guid companyId);
    }
}
