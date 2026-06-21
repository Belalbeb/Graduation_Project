using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Dtos
{
    public class UpdateInterviewDto
    {
        [Required]
        public string Date { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }
        [Required]
        public string InterviewLink { get; set; }
        [Required]
        public string InterviewerName { get; set; }
        [Required]
        public string InterviewType { get; set; }
    }
}
