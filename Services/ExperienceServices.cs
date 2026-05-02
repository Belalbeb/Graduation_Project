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

        public async Task<Experience?> GetExperienceByIdAsync(int experienceId)
        {
            return await _repository.GetByIdAsync(experienceId);
        }

        public async Task<List<ExperienceResponseDto>> GetAllAsync(int applicantId)
        {
            var experiences = await _repository.GetAllByApplicantAsync(applicantId);

            return experiences.Select(e => new ExperienceResponseDto
            {
                ExperienceID = e.ExperienceID,
                CompanyName  = e.CompanyName,
                Location     = e.Location,
                JobTitle     = e.JobTitle,
                Description  = e.Description,
                JobType      = e.JobType,
                StartDate    = e.StartDate,
                EndDate      = e.EndDate,
                ApplicantID  = e.ApplicantID
            }).ToList();
        }

        public async Task<ExperienceResponseDto?> AddExperienceAsync(Experience experience)
        {
            if (!await IsValidExperienceAsync(experience))
                return null;

            var added = await _repository.AddAsync(experience);

            return new ExperienceResponseDto
            {
                ExperienceID = added.ExperienceID,
                CompanyName  = added.CompanyName,
                Location     = added.Location,
                JobTitle     = added.JobTitle,
                Description  = added.Description,
                JobType      = added.JobType,
                StartDate    = added.StartDate,
                EndDate      = added.EndDate,
                ApplicantID  = added.ApplicantID
            };
        }

        public async Task<bool> DeleteExperienceAsync(int experienceId)
        {
            var experience = await _repository.GetByIdAsync(experienceId);
            if (experience == null) return false;

            await _repository.DeleteAsync(experience);
            return true;
        }

        public async Task<int> UpdateExperienceAsync(int experienceId, ExperienceDto experienceDto)
        {
            var experience = await _repository.GetByIdAsync(experienceId);
            if (experience == null) return 1; // not found

            experience.CompanyName = experienceDto.CompanyName;
            experience.Location    = experienceDto.Location;
            experience.JobTitle    = experienceDto.JobTitle;
            experience.Description = experienceDto.Description;
            experience.JobType     = experienceDto.JobType;
            experience.StartDate   = experienceDto.StartDate;
            experience.EndDate     = experienceDto.EndDate;

            if (!await IsValidExperienceAsync(experience))
                return 2; // dates conflict

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
