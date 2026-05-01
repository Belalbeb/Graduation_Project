using Graduation_Project.Dtos;
using Graduation_Project.Models;

namespace Graduation_Project.Services
{
    public interface IProfileService
    {
        Task<(string profileId, string profileType)> GetProfileAsync(ApplicationUser user);

        Task<PublicProfileDto?> GetPublicProfileAsync(int applicantId);
    }
}
