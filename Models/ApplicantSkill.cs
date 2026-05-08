namespace Graduation_Project.Models
{
    public class ApplicantSkill
    {
        public Guid ApplicantSkillID { get; set; } = Guid.NewGuid();

        public Guid ApplicantID { get; set; }
        public Applicant Applicant { get; set; } = null!;

        public Guid SkillID { get; set; }
        public Skill Skill { get; set; } = null!;

        public string ProficiencyLevel { get; set; } = string.Empty;
    }
}
