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

        public async Task<InterviewStatisticsDto> GetStatisticsAsync(int applicantId)
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

        public async Task<List<InterviewResponseDto>> GetUpcomingAsync(int applicantId)
        {
            var interviews = await _repository.GetByApplicantAndStatusAsync(applicantId, InterviewStatus.Upcoming);
            return MapToDto(interviews);
        }

        public async Task<List<InterviewResponseDto>> GetCompletedAsync(int applicantId)
        {
            var interviews = await _repository.GetByApplicantAndStatusAsync(applicantId, InterviewStatus.Completed);
            return MapToDto(interviews);
        }

        public async Task<List<InterviewResponseDto>> GetCancelledAsync(int applicantId)
        {
            var interviews = await _repository.GetByApplicantAndStatusAsync(applicantId, InterviewStatus.Cancelled);
            return MapToDto(interviews);
        }

        public async Task<List<InterviewResponseDto>> GetAllAsync(int applicantId)
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
    }
}
