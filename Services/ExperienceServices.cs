using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ExperienceServices : IExperienceService
    {
        private readonly IExperienceRepository _repository;

        public ExperienceServices(IExperienceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Experience?> GetExperienceByIdAsync(Guid experienceId)
        {
            return await _repository.GetByIdAsync(experienceId);
        }

        public async Task<List<ExperienceResponseDto>> GetAllAsync(Guid applicantId)
        {
            var experiences = await _repository.GetAllByApplicantAsync(applicantId);

            return experiences.Select(e => new ExperienceResponseDto
            {
                ExperienceID = e.ExperienceID,
                CompanyName  = e.CompanyName,
                Location     = e.Location,
                JobTitle     = e.JobTitle,
                Description  = e.Description,
                JobType      = e.JobType.ToString(),
                StartDate    = e.StartDate,
                EndDate      = e.EndDate,
                
            }).ToList();
        }

        public async Task<ExperienceResponseDto?> AddExperienceAsync(Experience experience)
        {

            var added = await _repository.AddAsync(experience);

            return new ExperienceResponseDto
            {
                ExperienceID = added.ExperienceID,
                CompanyName  = added.CompanyName,
                Location     = added.Location,
                JobTitle     = added.JobTitle,
                Description  = added.Description,
                JobType      = added.JobType.ToString(),
                StartDate    = added.StartDate,
                EndDate      = added.EndDate,
                ApplicantID  = added.ApplicantID
            };
        }

        public async Task<bool> DeleteExperienceAsync(Guid experienceId)
        {
            var experience = await _repository.GetByIdAsync(experienceId);
            if (experience == null) return false;

            await _repository.DeleteAsync(experience);
            return true;
        }

        public async Task<int> UpdateExperienceAsync(Guid experienceId, UpdateExperienceDto experienceDto)
        {
            var experience = await _repository.GetByIdAsync(experienceId);

            if (experience == null)
                return 1; // not found

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(experienceDto.CompanyName))
                experience.CompanyName = experienceDto.CompanyName;

            if (!string.IsNullOrWhiteSpace(experienceDto.Location))
                experience.Location = experienceDto.Location;

            if (!string.IsNullOrWhiteSpace(experienceDto.JobTitle))
                experience.JobTitle = experienceDto.JobTitle;

            if (!string.IsNullOrWhiteSpace(experienceDto.Description))
                experience.Description = experienceDto.Description;

            if (!string.IsNullOrWhiteSpace(experienceDto.JobType) &&
    Enum.TryParse<JobType>(experienceDto.JobType, true, out var jobType))
            {
                experience.JobType = jobType;
            }

            if (experienceDto.StartDate.HasValue)
                experience.StartDate = experienceDto.StartDate.Value;

            if (experienceDto.EndDate.HasValue)
                experience.EndDate = (experienceDto.EndDate).Value;

           

            await _repository.UpdateAsync(experience);

            return 3; // success
        }

        private async Task<bool> IsValidExperienceAsync(Experience experience)
        {
            if (experience.StartDate > experience.EndDate)
                return false;

            return !await _repository.HasOverlappingExperienceAsync(experience);
        }
    }
}
