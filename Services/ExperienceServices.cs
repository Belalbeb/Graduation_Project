using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Services
{
    public class ExperienceServices : IExperienceService
    {
        private readonly ApplicationDbContext _context ;
        public ExperienceServices(ApplicationDbContext context)
        {
            _context = context ;
        }
        public async Task<Experience?> GetExperienceByIdAsync(int experienceId)
        {
            return await _context.Experiences
                .FirstOrDefaultAsync(e => e.ExperienceID == experienceId) ;
        }
        public async Task<List<ExperienceResponseDto>> GetAllAsync(int applicantId)
        {
            return await _context.Experiences
                .Where(e => e.ApplicantID == applicantId)
                .OrderByDescending(e => e.StartDate)
                .Select(e => new ExperienceResponseDto
                {
                    ExperienceID = e.ExperienceID,
                    CompanyName = e.CompanyName,
                    Location = e.Location,
                    JobTitle = e.JobTitle,
                    Description = e.Description,
                    JobType = e.JobType,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    ApplicantID = e.ApplicantID
                })
                .ToListAsync() ;
        }
        public async Task<ExperienceResponseDto?> AddExperienceAsync(Experience experience)
        {
            if (!await IsValidExperienceAsync(experience))
                return null ;

            await _context.Experiences.AddAsync(experience) ;
            await _context.SaveChangesAsync() ;


            return new ExperienceResponseDto
            {
                ExperienceID = experience.ExperienceID,
                CompanyName = experience.CompanyName,
                Location = experience.Location,
                JobTitle = experience.JobTitle,
                Description = experience.Description,
                JobType = experience.JobType,
                StartDate = experience.StartDate,
                EndDate = experience.EndDate,
                ApplicantID = experience.ApplicantID
            } ;
        }

        public async Task<bool> DeleteExperienceAsync(int experienceId)
        {
            var experience = await _context.Experiences
                .FirstOrDefaultAsync(e => e.ExperienceID == experienceId) ;
            if (experience == null)
                return false ;

            _context.Experiences.Remove(experience) ;
            await _context.SaveChangesAsync() ;
            return true ;
        }


        public async Task<int> UpdateExperienceAsync(int experienceId, ExperienceDto experienceDto)
        {
            var experience = await _context.Experiences
                .FirstOrDefaultAsync(e => e.ExperienceID == experienceId) ;
            if (experience == null)
                return 1 ; // 1 --> experience not found

            experience.CompanyName = experienceDto.CompanyName ;
            experience.Location = experienceDto.Location ;
            experience.JobTitle = experienceDto.JobTitle ;
            experience.Description = experienceDto.Description ;
            experience.JobType = experienceDto.JobType ;
            experience.StartDate = experienceDto.StartDate ;
            experience.EndDate = experienceDto.EndDate ;

            var valid = await IsValidExperienceAsync(experience) ;
            if (!valid)
                return 2 ; // 2 --> experience found but dates confilct

            await _context.SaveChangesAsync() ;
            return 3 ; // 3 --> experience updated successfully
        }

        private async Task<bool> IsValidExperienceAsync(Experience experience)
        {
            if (experience.StartDate > experience.EndDate)
                return false ;

            return !await _context.Experiences
                .AnyAsync(e => e.ApplicantID == experience.ApplicantID &&
                        e.ExperienceID != experience.ExperienceID &&   // in case of update
                        e.StartDate < experience.EndDate &&
                        e.EndDate > experience.StartDate) ;
        }
    }
}
