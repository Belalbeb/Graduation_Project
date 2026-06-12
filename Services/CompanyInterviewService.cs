using Graduation_Project.Dtos.Company.Interview;
using Graduation_Project.Models;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class CompanyInterviewService : ICompanyInterviewService
    {
        private readonly IInterviewRepository _interviewRepository;

        public CompanyInterviewService(IInterviewRepository interviewRepository)
        {
            _interviewRepository = interviewRepository;
        }

        public async Task<CompanyInterviewStatisticsDto> GetStatisticsAsync(Guid companyId)
        {
            return await _interviewRepository.GetCompanyStatisticsAsync(companyId);
        }

        public async Task<List<CompanyInterviewListDto>> GetInterviewsAsync(Guid companyId,string? search = null,string? status = null)
        {
            var interviews = await _interviewRepository.GetCompanyInterviewsAsync(companyId,search,status);

            return interviews.Select(i => new CompanyInterviewListDto
            {
                InterviewId = i.InterviewId,
                CandidateName = $"{i.Applicant.FirstName} {i.Applicant.LastName}".Trim(),
                JobTitle = i.JobPosting.Title,
                ScheduledAt = i.ScheduledAt,
                Status = i.Status.ToString(),
                InterviewerName = i.InterviewerName
            }).ToList();
        }

        public async Task<CompanyInterviewDetailsDto?> GetInterviewDetailsAsync(Guid interviewId,Guid companyId)
        {
            var interview = await _interviewRepository.GetCompanyInterviewByIdAsync(interviewId,companyId);
            if(interview == null) return null;

            return new CompanyInterviewDetailsDto
            {
                InterviewId = interview.InterviewId,
                ApplicantId = interview.ApplicantId,
                CandidateName = $"{interview.Applicant.FirstName} {interview.Applicant.LastName}".Trim(),
                CandidateEmail = interview.Applicant.Email ?? "",
                JobTitle = interview.JobPosting.Title,
                ScheduledAt = interview.ScheduledAt,
                Status = interview.Status.ToString(),
                InterviewerName = interview.InterviewerName,
                InterviewType = interview.InterviewType,
                MeetingLink = interview.MeetingLink,
                Notes = interview.Notes
            };
        }

        public async Task<bool> UpdateInterviewAsync(Guid interviewId,Guid companyId,UpdateCompanyInterviewDto dto)
        {
            var interview = await _interviewRepository.GetCompanyInterviewByIdAsync(interviewId,companyId);
            if(interview == null) return false;

            if(!string.IsNullOrEmpty(dto.Status) &&
                Enum.TryParse<InterviewStatus>(dto.Status,true,out var newStatus))
            {
                interview.Status = newStatus;
            }

            if(dto.ScheduledAt.HasValue)
                interview.ScheduledAt = dto.ScheduledAt.Value;

            interview.InterviewerName = dto.InterviewerName;
            interview.InterviewType = dto.InterviewType;
            interview.MeetingLink = dto.MeetingLink;
            interview.Notes = dto.Notes;
            interview.UpdatedAt = DateTime.UtcNow;

            await _interviewRepository.UpdateAsync(interview);

            return true;
        }
    }
}
