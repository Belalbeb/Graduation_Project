using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Models
{
    public class ApplicationUser:IdentityUser
    {
        //public readonly string  Type { get; set; } // "Applicant" or "Company"
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsBlocked { get; set; }

        // Relations
        //public Applicant? Applicant { get; set; }
        public Company? Company { get; set; }
    }
}
