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
                ApplicantId  = application.ApplicantID,
                ImageUrl=application.Applicant.ProfilePicURL,
                CvPath = application.Resume.FilePath,
                CVName=application.Resume.FileName,
                ApplicationStatus = application.ApplicationStatus.ToString()


            };
            return applicantCompanyDto;
        }
        public async Task<bool> ChangeApplicationStatus(Guid ApplicationId, Guid companyId, ApplicationStatus status)
        {
            var result =await _repository.ChangeApplicationStatus(ApplicationId, companyId, status);
            if (!result) return false;
            return true;

        }
        public async Task<Application> CreateApplication(Guid ApplicantId, CreateApplicationDto createApplicationDto)
        {
            Application application = new Application()
            {
                ApplicantID = ApplicantId,
                ResumeID = createApplicationDto.ResumeID,
                JobPostingID = createApplicationDto.JobPostingID
            };
            await _repository.AddApplication(application);
            return application;
        }
    }
}
