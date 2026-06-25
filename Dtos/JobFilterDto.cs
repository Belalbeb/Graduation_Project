namespace Graduation_Project.Dtos
{
    public class JobFilterDto
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Category { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } =13 ;
    }
}
