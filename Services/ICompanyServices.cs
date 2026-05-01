using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface ICompanyServices
    {
        Task<Company> AddCompanyAsync(Company company);
        Task<Company?> GetCompanyByIdAsync(int id);
      
        Task<bool> UpdateCompanyAsync(int id, Company updatedCompany);
        Task<bool> DeleteCompanyAsync(int id);
        public Task<CompanyResponseDto?> GetCompanyDashboardAsync(int userId);
    }
}
