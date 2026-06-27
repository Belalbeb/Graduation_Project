using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public interface ICompanyVerificationRepository
    {
        Task AddAsync(CompanyVerificationRequest request);
        Task<CompanyVerificationRequest?> GetByIdAsync(Guid id);

        Task<CompanyVerificationRequest?> GetByIdWithCompanyAsync(Guid id);

        Task<List<CompanyVerificationRequest>> GetAllRequestAsync();

        Task UpdateAsync(CompanyVerificationRequest request);
        Task<CompanyVerificationRequest?> GetByCompanyAsync(Guid companyId);

        Task SaveChangesAsync();
    }
}
