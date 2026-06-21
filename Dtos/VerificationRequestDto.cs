namespace Graduation_Project.Dtos
{
    public class VerificationRequestDto
    {
        public Guid Id { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string Email { get; set; }
        public string? Location { get; set; }
        public string? Logo { get; set; }
        public string? Industry { get; set; }


        public string Status { get; set; } = string.Empty;

        public int DocumentsLenght { get; set; } 



        public DateTime CreatedAt { get; set; }
    }
}
