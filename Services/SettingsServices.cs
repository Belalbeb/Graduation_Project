using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class SettingsServices : ISettingsService
    {
        private readonly ISettingsRepository _repository;
        private readonly CloudinaryService _cloudinaryService;

        public SettingsServices(ISettingsRepository repository, CloudinaryService cloudinaryService)
        {
            _repository = repository;
            this._cloudinaryService = cloudinaryService;
        }

        public async Task<SettingsProfileDto?> GetProfileDetailsAsync(Guid applicantId)
        {
            var applicant = await _repository.GetApplicantByIdAsync(applicantId);
            if (applicant == null) return null;

            var resume = await _repository.GetActiveResumeAsync(applicantId);

            return new SettingsProfileDto
            {
                FullName      = applicant.FirstName + " " + applicant.LastName,
                JobTitle      = applicant.JobTitle ?? string.Empty,
                AboutMe       = applicant.AboutMe ?? string.Empty,
                ProfilePicUrl = applicant.ProfilePicURL,
                CoverPhotoUrl = applicant.CoverPhotoUrl,
                ResumeUrl     = resume?.FilePath
            };
        }

        public async Task<SettingsContactDto> GetContactDetailsAsync(Guid applicantId)
        {
            var applicant = await _repository.GetApplicantByIdAsync(applicantId);

            return new SettingsContactDto
            {
                Email     = applicant.User.Email ?? string.Empty,
                Phone     = applicant.PhoneNumber ?? string.Empty,
                Address   = applicant.Location ?? string.Empty,
                Linkedin  = applicant.Linkedin,
                Github    = applicant.Github,
                Facebook  = applicant.Facebook,
                Portfolio = applicant.Portfolio,
                Behance=applicant.Behance,
                Dribble=applicant.Dribble
                
            };
        }

        public async Task<bool> UpdateProfileAsync(Guid applicantId, UpdateProfileDto dto)
        {
            var applicant = await _repository.GetApplicantByIdAsync(applicantId);

            if (applicant == null)
                return false;

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                applicant.FirstName = string.Empty;
                applicant.LastName = string.Empty;
            }
            else
            {
                var nameParts = dto.FullName.Trim().Split(' ', 2);

                applicant.FirstName = nameParts[0];
                applicant.LastName = nameParts.Length > 1
                    ? nameParts[1]
                    : string.Empty;
            }

            applicant.JobTitle = dto.JobTitle;
            applicant.AboutMe = dto.AboutMe;
            applicant.Location = dto.Country;
            applicant.Industry = dto.Industry;

            await _repository.UpdateApplicantAsync(applicant);

            return true;
        }
        public async Task<bool> updatePhoto(Guid applicantId,IFormFile photo)
        {
            var applicant = await _repository.GetApplicantByIdAsync(applicantId);

            var profileUrl =
                    await _cloudinaryService.UploadImageAsync(photo);

                applicant.ProfilePicURL = profileUrl;
            await _repository.UpdateApplicantAsync(applicant);
            return true;
          




        }
        public async Task<bool> UpdateCoverPhoto(Guid applicantId, IFormFile coverPhoto)
        {
            var applicant = await _repository.GetApplicantByIdAsync(applicantId);

            if (applicant == null)
                return false;

            var coverUrl = await _cloudinaryService.UploadImageAsync(coverPhoto);

            applicant.CoverPhotoUrl = coverUrl;

            await _repository.UpdateApplicantAsync(applicant);

            return true;
        }
        public async Task<bool> UpdateResume(Guid applicantId, IFormFile resume,string resumeName)
        {
            var applicant = await _repository.GetApplicantByIdAsync(applicantId);

            if (applicant == null)
                return false;

            var resumeUrl = await _cloudinaryService.UploadFileAsync(resume);

            await UpdateActiveResumeAsync(applicantId, resumeUrl,resumeName);

            await _repository.UpdateApplicantAsync(applicant);

            return true;
        }

        public async Task<bool> UpdateContactAsync(Guid applicantId, UpdateContactDto dto)
        {
            var applicant = await _repository.GetApplicantByIdAsync(applicantId);

            if (applicant == null)
                return false;

            applicant.PhoneNumber = dto.Phone;

            applicant.Linkedin = dto.Linkedin;
            applicant.Behance = dto.Behance;
            applicant.Dribble = dto.Dribbble;
            applicant.Address = dto.Address;
            applicant.Github = dto.Github;
            applicant.Facebook = dto.Facebook;
            applicant.Portfolio = dto.Portfolio;

            await _repository.UpdateApplicantAsync(applicant);

            return true;
        }
        private async Task UpdateActiveResumeAsync(Guid applicantId, string resumeUrl,string fileName)
        {
            await _repository.DeactivateAllResumesAsync(applicantId);

            var newResume = new Resume
            {
                FileName    = fileName,
                FilePath    = resumeUrl,
                UploadDate  = DateTime.UtcNow,
                IsActive    = true,
                ApplicantID = applicantId
            };

            await _repository.AddResumeAsync(newResume);
        }
    }
}
