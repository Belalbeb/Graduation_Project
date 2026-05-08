using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class JobMetric
    {
        [Key]
        public Guid MetricID { get; set; } = Guid.NewGuid();
        public int Views { get; set; }
        public int ApplicationCount { get; set; }
        public DateTime LastUpdated { get; set; }

        // FK
        public Guid JobID { get; set; }
        public JobPosting JobPosting { get; set; }
    }
}
