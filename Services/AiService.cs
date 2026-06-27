namespace Graduation_Project.Services
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;

        public AiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CalculateMatchScoreAsync(Guid applicationId)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "sync-application-match-score",
                new
                {
                    application_id = applicationId
                });

            response.EnsureSuccessStatusCode();
        }
    }
}
