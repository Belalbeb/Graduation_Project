namespace Graduation_Project.Services
{
    public interface IAiService
    {
        Task CalculateMatchScoreAsync(Guid applicationId);
    }
}
