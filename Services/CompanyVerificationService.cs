using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class CompanyVerificationService : ICompanyVerificationService
    {
        private readonly ICompanyVerificationRepository _repo;
        private readonly ICompanyRepository _companyRepo;
        private readonly CloudinaryService _cloudinaryService;

        public CompanyVerificationService(
            ICompanyVerificationRepository repo,
            ICompanyRepository companyRepo,
            CloudinaryService cloudinaryService)
        {
            _repo = repo;
            _companyRepo = companyRepo;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<bool> CreateRequestAsync(Guid companyId, List<IFormFile> documents)
        {
            var company = await _companyRepo.GetByIdAsync(companyId);

            if (company == null)
                return false;

            var request = new CompanyVerificationRequest
            {
                CompanyId = companyId,
                Status = VerificationStatus.Pending
            };

            foreach (var doc in documents)
            {
                var url = await _cloudinaryService.UploadFileAsync(doc);

                request.Documents.Add(new VerificationDocument
                {
                    FileName = doc.FileName,
                    FileUrl = url,
                    FileSize=doc.Length
                });
            }

            await _repo.AddAsync(request);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<List<VerificationRequestDto>> GetAllRequestsAsync()
        {
            var comapnyVertificationRequest= await _repo.GetAllRequestAsync();
            var comapnyVertificationRequestDto = comapnyVertificationRequest.Select(x => new VerificationRequestDto
            {
                Id = x.Id,
               
                CompanyName = x.Company.Name,
                Email = x.Company.User.Email,
                Location =x.Company.Location,
                Logo=x.Company.LogoUrl,
                Industry=x.Company.Industry,

                Status = x.Status.ToString(),

                DocumentsLenght = x.Documents.Count,

                CreatedAt = x.SubmittedAt
            }).ToList();
            return comapnyVertificationRequestDto;
        }
        public async Task<VertificationRequestDetailsDto> GetVertificationRequestDetails(Guid VertificationId)
        {
            var VertificationRequest = await _repo.GetByIdAsync(VertificationId);
            if (VertificationRequest == null) return null;
            var VertificationRequestDetailsDto = new VertificationRequestDetailsDto()
            {
                CompanyId = VertificationRequest.CompanyId,
                CompanyName = VertificationRequest.Company.Name,
                CompanyCoverImage = VertificationRequest.Company.CoverLogoUrl,
                CompanyLogoImage = VertificationRequest.Company.LogoUrl,
                CompanyDescription = VertificationRequest.Company.Description,
                Status = VertificationRequest.Status.ToString(),
                Documents = VertificationRequest.Documents.Select(doc => new VerificationDocumentDto
                {
                    FileUrl = doc.FileUrl,
                    FileSize = doc.FileSize,
                    FileName = doc.FileName
                }).ToList(),
                Notes=VertificationRequest.AdminNotes,
                CreatedAt = VertificationRequest.SubmittedAt

            };
            return VertificationRequestDetailsDto;




        }
        public async Task<bool> ApproveAsync(Guid requestId)
        {
            var request = await _repo.GetByIdWithCompanyAsync(requestId);

            if (request == null)
                return false;

            request.Status = VerificationStatus.Approved;
         

            request.Company.Status = CompanyStatus.Verified;

            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectAsync(Guid requestId)
        {
            var request = await _repo.GetByIdAsync(requestId);

            if (request == null)
                return false;

            request.Status = VerificationStatus.Rejected;
         
            

            await _repo.UpdateAsync(request);
            await _repo.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RequestMoreInformationAsync(Guid requestId, string notes)
        {
            var request = await _repo.GetByIdAsync(requestId);

            if (request == null)
                return false;

            request.Status = VerificationStatus.NeedsMoreInformation;
            request.AdminNotes = notes;

            await _repo.SaveChangesAsync();

            return true;
        }
    }
}
