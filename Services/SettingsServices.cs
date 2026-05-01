using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class SettingsServices : ISettingsService
    {
        private readonly ApplicationDbContext _context ;

        public SettingsServices(ApplicationDbContext context)
        {
            _context = context ;
        }

        public async Task<SettingsProfileDto?> GetProfileDetailsAsync(int applicantId)
        {
            var applicant = await _context.Applicants.FindAsync(applicantId) ;
            if (applicant == null)
                return null;

            var resume = await _context.Resumes
                .FirstOrDefaultAsync(r => r.ApplicantID == applicantId && r.IsActive) ;

            return new SettingsProfileDto
            {
                FullName = applicant.FirstName + " " + applicant.LastName,
                JobTitle = applicant.JobTitle ?? string.Empty,
                AboutMe = applicant.AboutMe ?? string.Empty,
                ProfilePicUrl = applicant.ProfilePicURL,
                CoverPhotoUrl = applicant.CoverPhotoUrl,
                ResumeUrl = resume?.FilePath
            } ;
        }
        public async Task<SettingsContactDto> GetContactDetailsAsync(int applicantId)
        {
            var applicant = await _context.Applicants.FindAsync(applicantId) ;

            return new SettingsContactDto
            {
                Email = applicant.Email ?? string.Empty,
                Phone = applicant.PhoneNumber ?? string.Empty,
                Address = applicant.Location ?? string.Empty,
                Linkedin = applicant.Linkedin,
                Github = applicant.Github,
                Facebook = applicant.Facebook,
                Portfolio = applicant.Portfolio
            };
        }

        public async Task<bool> UpdateProfileAsync(int applicantId,UpdateProfileDto dto)
        {
            var applicant = await _context.Applicants.FindAsync(applicantId);
            if(applicant == null)
                return false;

            bool hasChanges = false;

            if(!string.IsNullOrWhiteSpace(dto.FullName))
            {
                var nameParts = dto.FullName.Trim().Split(' ',2);
                applicant.FirstName = nameParts[0];
                applicant.LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
                hasChanges = true;
            }

            if(!string.IsNullOrWhiteSpace(dto.JobTitle))
            {
                applicant.JobTitle = dto.JobTitle;
                hasChanges = true;
            }

            if(dto.AboutMe != null)
            {
                applicant.AboutMe = dto.AboutMe;
                hasChanges = true;
            }

            if(!string.IsNullOrWhiteSpace(dto.ProfilePicUrl))
            {
                applicant.ProfilePicURL = dto.ProfilePicUrl;
                hasChanges = true;
            }

            if(!string.IsNullOrWhiteSpace(dto.CoverPhotoUrl))
            {
                applicant.CoverPhotoUrl = dto.CoverPhotoUrl;
                hasChanges = true;
            }

            if(!string.IsNullOrWhiteSpace(dto.ResumeUrl))
            {
                await UpdateActiveResumeAsync(applicantId,dto.ResumeUrl);
                hasChanges = true;
            }

            if(hasChanges)
            {
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateContactAsync(int applicantId,UpdateContactDto dto)
        {
            var applicant = await _context.Applicants.FindAsync(applicantId);
            if(applicant == null)
                return false;

            if(!string.IsNullOrEmpty(dto.Email)) applicant.Email = dto.Email;
            if(!string.IsNullOrEmpty(dto.Phone)) applicant.PhoneNumber = dto.Phone;
            if(!string.IsNullOrEmpty(dto.Address)) applicant.Location = dto.Address;
            if(dto.Linkedin != null) applicant.Linkedin = dto.Linkedin;
            if(dto.Github != null) applicant.Github = dto.Github;
            if(dto.Facebook != null) applicant.Facebook = dto.Facebook;
            if(dto.Portfolio != null) applicant.Portfolio = dto.Portfolio;

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task UpdateActiveResumeAsync(int applicantId,string resumeUrl)
        {
            // Deactivate all previous resumes
            await _context.Resumes
                .Where(r => r.ApplicantID == applicantId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(r => r.IsActive,false));

            var newResume = new Resume
            {
                FileName = "Uploaded_CV.pdf",
                FilePath = resumeUrl,
                UploadDate = DateTime.UtcNow,
                IsActive = true,
                ApplicantID = applicantId
            };

            await _context.Resumes.AddAsync(newResume);
            await _context.SaveChangesAsync();
        }
    }
}
