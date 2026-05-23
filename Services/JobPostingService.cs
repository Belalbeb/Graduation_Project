using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class JobPostingService : IJobPostingService
    {
        private readonly IJobPostingRepository _repository;

        public IInterviewRepository InterviewRepository { get; }

        public JobPostingService(IJobPostingRepository repository,IInterviewRepository interviewRepository)
        {
            _repository = repository;
            InterviewRepository = interviewRepository;
        }

        public async Task<IEnumerable<JobPosting>> GetAllJobsAsync()
        {
            return await _repository.GetAllAsync();
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
              .SelectMany(x => x.Applications ?? new List<Application>())
              .Select(a => a.ApplicantID)
               .Distinct()
               .Count(); 

            var jobDetails = jobsList.Select(job => new JobDetails
            {
                JobId = job.JobID,
                JobTitle = job.Title,
                IsActive = job.IsActive,
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
                IsActive = job.IsActive,
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
                        ApplicantId=x.ApplicationID,
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
                        InterviewDate = i.ScheduledAt,
                        InterviewStatus = i.Status.ToString()
                    }).ToList()
            };
        }

        public async Task<JobPosting> UpdateJobAsync(Guid id, JobPosting jobPosting)
        {
            return await _repository.UpdateAsync(id, jobPosting);
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
