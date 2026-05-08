namespace Graduation_Project.Models
{
    public class Skill
    {
        public Guid SkillID { get; set; } = Guid.NewGuid();
        public string SkillName { get; set; }

        // Navigation
        public ICollection<ApplicantSkill> ApplicantSkills { get; set; }
    }
}
