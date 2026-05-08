namespace Graduation_Project.Dtos
{
    public class SkillResponseDto
    {
        public Guid ApplicantSkillID { get; set; }
        public Guid SkillID { get; set; }
        public string SkillName { get; set; } = string.Empty;
    }
}
