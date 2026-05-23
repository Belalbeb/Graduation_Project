using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class InterviewService : IInterviewServices
    {
        private readonly IInterviewRepository _repository;

        public InterviewService(IInterviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<InterviewStatisticsDto> GetStatisticsAsync(Guid applicantId)
        {
            var total     = await _repository.CountByApplicantAsync(applicantId);
            var upcoming  = await _repository.CountByApplicantAndStatusAsync(applicantId, InterviewStatus.Upcoming);
            var completed = await _repository.CountByApplicantAndStatusAsync(applicantId, InterviewStatus.Completed);
            var cancelled = await _repository.CountByApplicantAndStatusAsync(applicantId, InterviewStatus.Cancelled);

            return new InterviewStatisticsDto
            {
                Total     = total,
                Upcoming  = upcoming,
                Completed = completed,
                Cancelled = cancelled
            };
        }

        public async Task<List<InterviewResponseDto>> GetUpcomingAsync(Guid applicantId)
        {
            var interviews = await _repository.GetByApplicantAndStatusAsync(applicantId, InterviewStatus.Upcoming);
            return MapToDto(interviews);
        }

        public async Task<List<InterviewResponseDto>> GetCompletedAsync(Guid applicantId)
        {
            var interviews = await _repository.GetByApplicantAndStatusAsync(applicantId, InterviewStatus.Completed);
            return MapToDto(interviews);
        }

        public async Task<List<InterviewResponseDto>> GetCancelledAsync(Guid applicantId)
        {
            var interviews = await _repository.GetByApplicantAndStatusAsync(applicantId, InterviewStatus.Cancelled);
            return MapToDto(interviews);
        }
        public async Task<InterviewCompanyResponseDto> InterviewCompanyDetails(Guid InterviewId)
        {
            var interview =await _repository.GetInterviewById(InterviewId);

            InterviewCompanyResponseDto responseDto = new InterviewCompanyResponseDto()
            {
                InterviewId=interview.InterviewId,
                ApplicantName = $"{interview.Applicant?.FirstName} {interview.Applicant?.LastName}",

                ImageUrl = interview.Applicant?.ProfilePicURL,

                Email = interview.Applicant?.Email,

                PositionTitle = interview.JobPosting?.Title,

                ResumePath = interview.Applicant?
        .Resumes?
        .FirstOrDefault(x => x.IsActive)?
        .FilePath,

                InterviewStatus = interview.Status.ToString(),

                InterviewDate = interview.ScheduledAt,

                InterviewerName = interview.InterviewerName,

                InterviewerPosition = interview.InterviewerPosition,

                InterviewLink = interview.MeetingLink
            };
            return responseDto;
        }
        public async Task<List<InterviewResponseDto>> GetAllAsync(Guid applicantId)
        {
            var interviews = await _repository.GetAllByApplicantAsync(applicantId);
            return MapToDto(interviews);
        }

        private static List<InterviewResponseDto> MapToDto(List<Interview> interviews)
        {
            return interviews.Select(i => new InterviewResponseDto
            {
                InterviewId     = i.InterviewId,
                JobPostingId    = i.JobPostingId,
                JobTitle        = i.JobPosting.Title,
                CompanyName     = i.JobPosting.Company.Name,
                CompanyLogoUrl  = i.JobPosting.Company.LogoUrl,
                ScheduledAt     = i.ScheduledAt,
                Status          = i.Status,
                InterviewerName = i.InterviewerName,
                MeetingLink     = i.MeetingLink,
                Notes           = i.Notes
            }).ToList();
        }
        public async Task<bool> ChangeInterviewDate(Guid InterviewId,DateTime InterviewDate)
        {
           return await _repository.ChangeInterviewDate(InterviewId, InterviewDate);
                
                


        }

    }
}
