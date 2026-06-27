using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface ICompanyVerificationService
    {
        Task<bool> CreateRequestAsync(Guid companyId, List<IFormFile> documents);
        Task<VerificationRequestStatusDtoForCompany?> GetVerificationRequestStatusAsync(Guid companyId);
        Task<List<VerificationRequestDto>> GetAllRequestsAsync();
     Task<VertificationRequestDetailsDto> GetVertificationRequestDetails(Guid VertificationId);

        Task<bool> ApproveAsync(Guid requestId);
        Task<bool> RequestMoreInformationAsync(Guid requestId, string notes);

        Task<bool> RejectAsync(Guid requestId);
    }
}
