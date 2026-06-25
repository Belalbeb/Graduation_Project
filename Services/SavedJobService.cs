using Graduation_Project.Dtos;
using Graduation_Project.Exceptions;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class SavedJobService : ISavedJobService
    {
        private readonly ISavedJobRepository _repository;

        public SavedJobService(
            ISavedJobRepository repository)
        {
            _repository = repository;
        }

        public async Task SaveJobAsync(
            Guid applicantId,
            Guid jobId)
        {
            var exists = await _repository
                .ExistsAsync(applicantId, jobId);

            if (exists)
                throw new BadRequestException(
                    "Job already saved");

            await _repository.AddAsync(new SavedJobs
            {
                ApplicantId = applicantId,
                JobPostingId = jobId
            });
        }

        public async Task<List<JobCardDto>> GetSavedJobsAsync(Guid applicantId)
        {
            var savedJobs = await _repository
                .GetSavedJobsAsync(applicantId);

            return savedJobs.Select(x => new JobCardDto
            {
                JobID = x.JobPosting.JobID,

                CompanyName = x.JobPosting.Company.Name,
                CompanyLogoUrl = x.JobPosting.Company.LogoUrl,

                Location = x.JobPosting.Location,
                Category = x.JobPosting.JobCategory,

                MinExperience = x.JobPosting.MinExperience,
                MaxExperience = x.JobPosting.MaxExperience,

                Title = x.JobPosting.Title,
                Description = x.JobPosting.Description,

                MinSalary = x.JobPosting.MinSalary,
                MaxSalary = x.JobPosting.MaxSalary,

                PostedDate = x.JobPosting.PostedDate,

                JobTypes = x.JobPosting.JobTypes
                    .Select(t => t.ToString())
                    .ToList(),

                WorkApproaches = x.JobPosting.WorkApproaches
                    .Select(w => w.ToString())
                    .ToList(),

                IsApplied = x.JobPosting.Applications
                    .Any(a => a.ApplicantID == applicantId)
            }).ToList();
        }
        public async Task UnsaveJobAsync(Guid applicantId,Guid jobId)
        {
            await _repository.UnsaveJobAsync(
                applicantId,
                jobId);
        }
    }
}
