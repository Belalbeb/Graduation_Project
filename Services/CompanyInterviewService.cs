using Graduation_Project.Dtos;
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

        public async Task<List<CompanyInterviewListDto>> GetInterviewsAsync(
            Guid companyId,
            string? search = null,
            string? status = null)
        {
            var interviews = await _interviewRepository
                .GetCompanyInterviewsAsync(companyId, search, status);

            return interviews.Select(i => new CompanyInterviewListDto
            {
                InterviewId = i.InterviewId,
                CandidateName = $"{i.Applicant.FirstName} {i.Applicant.LastName}".Trim(),
                ImageUrl=i.Applicant.ProfilePicURL,
                Email=i.Applicant.Email,
                JobId=i.JobPostingId,
                JobTitle = i.JobPosting.Title,

                ScheduledDate = i.InterviewDate,
                StartTime = i.StartTime,
                EndTime = i.EndTime,

                Status = i.Status.ToString(),
     
            }).ToList();
        }

        public async Task<CompanyInterviewDetailsDto?> GetInterviewDetailsAsync(
            Guid interviewId,
            Guid companyId)
        {
            var interview = await _interviewRepository
                .GetCompanyInterviewByIdAsync(interviewId, companyId);

            if (interview == null)
                return null;

            return new CompanyInterviewDetailsDto
            {
                InterviewId = interview.InterviewId,
                ApplicantId = interview.ApplicantId,
                CandidateName = $"{interview.Applicant.FirstName} {interview.Applicant.LastName}".Trim(),
                CandidateEmail = interview.Applicant.Email ?? "",
                JobTitle = interview.JobPosting.Title,

       
                InterviewDate = interview.InterviewDate,
                StartTime = interview.StartTime,
                EndTime = interview.EndTime,

                Status = interview.Status.ToString(),
                InterviewerName = interview.InterviewerName,

                InterviewType = interview.interviewType.ToString(),

                MeetingLink = interview.MeetingLink,
                Notes = interview.Notes
            };
        }

        public async Task<bool> UpdateInterviewAsync(
    Guid interviewId,Guid companyId,
    UpdateCompanyInterviewDto dto)
        {
            var interview = await _interviewRepository.GetCompanyInterviewByIdAsync(interviewId,companyId);

            if (interview == null)
                return false;

            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                Enum.TryParse<InterviewStatus>(dto.Status, true, out var status))
            {
                interview.Status = status;
            }

            if (!string.IsNullOrWhiteSpace(dto.InterviewDate))
            {
                interview.InterviewDate = DateOnly.Parse(dto.InterviewDate);
            }
            if (dto.StartTime.HasValue)
            {
                interview.StartTime = dto.StartTime.Value;
            }

            if (dto.EndTime.HasValue)
            {
                interview.EndTime = dto.EndTime.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.InterviewerName))
            {
                interview.InterviewerName = dto.InterviewerName;
            }

            if (!string.IsNullOrWhiteSpace(dto.InterviewType) &&
                Enum.TryParse<InterviewType>(dto.InterviewType, true, out var type))
            {
                interview.interviewType = type;
            }

            if (!string.IsNullOrWhiteSpace(dto.MeetingLink))
            {
                interview.MeetingLink = dto.MeetingLink;
            }

            if (dto.Notes != null)
            {
                interview.Notes = dto.Notes;
            }

            await _interviewRepository.UpdateAsync(interview);

            return true;
        }
        public async Task<Guid> ScheduleInterviewAsync(ScheduleInterviewDto dto)
        {
            var interview = new Interview();

            interview.InterviewDate = DateOnly.Parse(dto.Date);

        
            interview.StartTime = TimeOnly.Parse(dto.StartTime);
            interview.EndTime = TimeOnly.Parse(dto.EndTime);

            interview.Notes = dto.Notes;
            interview.MeetingLink = dto.InterviewLink;
            interview.InterviewerName = dto.InterviewerName;

            if (!Enum.TryParse<InterviewType>(dto.InterviewType.Replace(" Interview", ""), true, out var type))
            {
                type = InterviewType.Technical; 
            }

            interview.interviewType = type;

            interview.ApplicantId = dto.ApplicantId;
            interview.JobPostingId = dto.JobPostingId;

            await _interviewRepository.AddAsync(interview);
           

            return interview.InterviewId;
        }

    }
}