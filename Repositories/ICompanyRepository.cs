using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> AddAsync(Company company);
        Task<Company?> GetByIdAsync(int id);
        Task<Company?> GetByUserIdAsync(string userId);
        Task<Company?> GetWithJobPostingsAndApplicationsAsync(int companyId);
        Task<bool> UpdateAsync(Company company);
        Task<bool> DeleteAsync(int id);
    }
}
