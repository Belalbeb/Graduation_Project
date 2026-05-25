using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> AddAsync(Company company);
        Task<Company?> GetByIdAsync(Guid id);
        Task<Company?> GetByUserIdAsync(string userId);
        Task<Company?> GetWithJobPostingsAndApplicationsAsync(Guid companyId);
        Task<bool> UpdateAsync(Company company);
        Task<bool> DeleteAsync(Guid id);
<<<<<<< HEAD

        Task<Company?> GetCompanyForSettingsAsync(Guid companyId);
        Task<bool> UpdateCompanyProfileAsync(Company company);
=======
        Task<int> GetCompaniesCount();
>>>>>>> master
    }
}
