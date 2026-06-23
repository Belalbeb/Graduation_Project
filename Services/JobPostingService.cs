using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;
using Microsoft.Extensions.Caching.Memory;


namespace Graduation_Project.Services
{
    public class JobPostingService : IJobPostingService
    {
        private readonly IJobPostingRepository _repository;
        private readonly IMemoryCache cache;

        public IInterviewRepository InterviewRepository { get; }

        public JobPostingService(IJobPostingRepository repository,IInterviewRepository interviewRepository, IMemoryCache cache)
        {
            _repository = repository;
            InterviewRepository = interviewRepository;
            this.cache = cache;
        }
        public async Task<PagedResult<JobCardDto>> GetAllJobsAsync(Guid currentApplicantId,JobFilterDto jobFilterDto)
        {
            var result = await _repository.GetAllAsync(jobFilterDto);

            var items = result.Jobs.Select(job => new JobCardDto
            {
                JobID = job.JobID,
                CompanyName = job.Company?.Name,
                CompanyLogoUrl = job.Company?.LogoUrl,
                Location = job.Location,
                Category=job.JobCategory,
                Title = job.Title,
                Description = job.Description,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                PostedDate = job.PostedDate,

                JobTypes = job.JobTypes
                    .Select(t => t.ToString())
                    .ToList(),

                WorkApproaches = job.WorkApproaches
                    .Select(w => w.ToString())
                    .ToList(),

                IsApplied = currentApplicantId != Guid.Empty &&
                            job.Applications.Any(a =>
                                a.ApplicantID == currentApplicantId)
            }).ToList();

            return new PagedResult<JobCardDto>
            {
                Items = items,
                TotalCount = result.TotalCount,
                Page = jobFilterDto.Page,
                PageSize = jobFilterDto.PageSize
            };
        }

        public async Task<JobPosting> GetJobByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<JobPostingDto> GetJobsByCompanyAsync(Guid companyId)
        {
            var jobs = await _repository.GetByCompanyAsync(companyId);
            var jobsList = jobs.ToList();

            int jobPostingCount = jobsList.Count;
            int activeJobPostedCount = jobsList.Count(x => x.IsActive);
            int applicantCount = jobsList
              .SelectMany(x => x.Applications ?? new List<Graduation_Project.Models.Application>())
              .Select(a => a.ApplicantID)
               .Distinct()
               .Count(); 

            var jobDetails = jobsList.Select(job => new JobDetails
            {
                JobId = job.JobID,
                JobTitle = job.Title,
                JobStatus=job.Status.ToString(),
                Location = job.Location,
                PostedAt = job.PostedDate,
                JobType = job.JobTypes.Select(x=>x.ToString()).ToList(),
                ApplicationCount = job.Applications?.Count ?? 0
            }).ToList();

            return new JobPostingDto
            {
                JobPostingCount = jobPostingCount,
                ActiveJobPostedCount = activeJobPostedCount,
                ApplicantCount = applicantCount,
                JobDetails = jobDetails
            };
        }
     
        public async Task<JobPosting> CreateJobAsync(CreateJobDto dto,Guid companyId)
        {
            
            var jobPosting = new JobPosting
            {
                Title = dto.JobBasicData.JobTitle,
                Description = dto.JobDetails.JobDescription,
                Responsibility = dto.JobDetails.Responsibilities,
                MaxExperience=dto.JobBasicData.MaxExperience,
                MinExperience=dto.JobBasicData.MinExperience,

                MinSalary = dto.JobBasicData.SalaryMin,
                MaxSalary = dto.JobBasicData.SalaryMax,

                JobCategory = dto.JobBasicData.JobCategory,
                Location = dto.JobBasicData.Location,

                PostedDate = DateTime.Now,
                IsActive = true,

                
                JobTypes = dto.JobBasicData.EmploymentType
                    .Select(e => Enum.Parse<JobType>(
                        e.Replace("-", "").Replace(" ",""), true))
                    .ToList(),

                WorkApproaches = dto.JobBasicData.WorkApproach
                    .Select(w => Enum.Parse<WorkApproach>(
                        w.Replace("-", ""), true))
                    .ToList(),

              
                IsRemote = dto.JobBasicData.WorkApproach
                    .Any(w => w.Equals("Remote", StringComparison.OrdinalIgnoreCase)),

           
                Skills = dto.JobDetails.Skills
                    .Select(skill => new JobSkill
                    {
                        Name = skill
                    }).ToList(),

             
                CompanyID = companyId
            };

            return await _repository.AddAsync(jobPosting);
        }
        public async Task<JobInformationResponseDto> JobDetails(Guid jobId)
        {
            var job = await _repository.GetByIdAsync(jobId);

            if (job == null)
                return null;

            var interviews = await InterviewRepository.GetByjobPostingId(jobId);

            return new JobInformationResponseDto
            {
                Title = job.Title,
                JobStatus=job.Status.ToString(),
                WorkApproaches=job.WorkApproaches.Select(x=>x.ToString()).ToList(),
                JobTypes=job.JobTypes.Select(x=>x.ToString()).ToList(),
                IsActive=job.IsActive,
                MaxExper=job.MaxExperience,
                MinExper=job.MinExperience,

                Category = job.JobCategory,
                PostedDate = job.PostedDate,

                ApplicantsCount = job.Applications.Count,
                InterviewCount = job.Interviews.Count,

                Description = job.Description,
                Responsibility = job.Responsibility,

                RequiredSkill = job.Skills
                    .Select(s => s.Name)
                    .ToList(),

                Location = job.Location,

                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,

                ApplicantDetails = job.Applications
                    .Select(x => new ApplicantDetail
                    {
                        ApplicantionId=x.ApplicationID,
                        ApplicantName = $"{x.Applicant.FirstName} {x.Applicant.LastName}",
                        Email = x.Applicant.Email,
                        ImageUrl = x.Applicant.ProfilePicURL,
                        AppliedDate = x.AppliedDate,
                        Status = x.ApplicationStatus.ToString()
                    }).ToList(),

                ApplicantInterviews = interviews
                    .Select(i => new InterviewDetailDto
                    {
                        InterviewId=i.InterviewId,
                        ApplicantName = $"{i.Applicant.FirstName} {i.Applicant.LastName}",
                        Email = i.Applicant.Email,
                        ImageUrl = i.Applicant.ProfilePicURL,
                        InterviewDate = i.InterviewDate,
                        StartTime=i.StartTime,
                        EndTime=i.EndTime,
                        InterviewStatus = i.Status.ToString()
                    }).ToList()
            };
        }

        public async Task<bool> UpdateJobAsync(Guid id, UpdateJobDto dto)
        {
            var job = await _repository.GetByIdAsync(id);

            if (job == null)
                return false;

            job.IsActive = dto.IsActive;
            job.Title = dto.JobBasicData.JobTitle;
            job.JobCategory = dto.JobBasicData.JobCategory;
            job.Location = dto.JobBasicData.Location;
            job.MinSalary = dto.JobBasicData.SalaryMin;
            job.MaxSalary = dto.JobBasicData.SalaryMax;
            job.MinExperience = dto.JobBasicData.MinExperience;
            job.MaxExperience = dto.JobBasicData.MaxExperience;

            job.Description = dto.JobDetails.JobDescription;
            job.Responsibility = dto.JobDetails.Responsibilities;

            job.JobTypes = dto.JobBasicData.EmploymentType
                .Select(e => Enum.Parse<JobType>(e.Replace("-", "").Replace(" ", ""), true))
                .ToList();

            job.WorkApproaches = dto.JobBasicData.WorkApproach
                .Select(w => Enum.Parse<WorkApproach>(w.Replace("-", "").Replace(" ", ""), true))
                .ToList();

            job.IsRemote = dto.JobBasicData.WorkApproach
                .Any(w => w.Equals("Remote", StringComparison.OrdinalIgnoreCase));

            await _repository.ReplaceSkillsAsync(job.JobID, dto.JobDetails.Skills);

            return await _repository.UpdateAsync(job);
        }

        public async Task<bool> DeleteJobAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> DeactivateJobAsync(Guid id)
        {
            return await _repository.DeactivateAsync(id);
        }
      
    }
}
