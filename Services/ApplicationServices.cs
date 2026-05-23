using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ApplicationServices : IApplicationServivces
    {
        private readonly IApplicationRepository _repository;

        public ApplicationServices(IApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ApplicationDto>> GetApplicationByApplicant(Guid id)
        {
            return await _repository.GetByApplicantIdAsync(id);
        }

        public async Task<ApplicantCompanyDto> GetApplicantByApplication(Guid ApplicationId)
        {
            var application =await _repository.GetApplicationByIdAsync(ApplicationId);
            if (application == null) return null;
            ApplicantCompanyDto applicantCompanyDto = new ApplicantCompanyDto()
            {
                Name = $"{application.Applicant.FirstName} {application.Applicant.LastName}",
                Email = application.Applicant.Email,
                PortfolioLink = application.Applicant.ProfilePicURL,
                //CvPath = application.Resume.FilePath,
                ApplicationStatus = application.ApplicationStatus.ToString()


            };
            return applicantCompanyDto;
        }
        public async Task<bool> ChangeApplicationStatus(Guid ApplicationId,ApplicationStatus status)
        {
            var result =await _repository.ChangeApplicationStatus(ApplicationId, status);
            if (!result) return false;
            return true;

        }
    }
}
