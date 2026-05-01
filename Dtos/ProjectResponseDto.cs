namespace Graduation_Project.Dtos
{
    public class ProjectResponseDto
    {
        public int ProjectID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ProjectUrl { get; set; }
        public string? GithubRepoUrl { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
