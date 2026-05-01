using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Graduation_Project.Models
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions):base(contextOptions)
        {
            
        }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<ApplicantSkill> ApplicantSkills { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<JobMetric> JobMetrics { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<SavedJobs> SavedJobs { get; set; }
        public DbSet<ProfileView> ProfileViews { get; set; }
        public DbSet<Project> Projects { get ; set ;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
       //     builder.Entity<Applicant>()
       //.HasOne(a => a.User)
       //.WithOne(u => u.Applicant)
       //.HasForeignKey<Applicant>(a => a.UserId)
       //.OnDelete(DeleteBehavior.NoAction);

            // Company ↔ User (One-to-One)
            builder.Entity<Company>()
                .HasOne(c => c.User)
                .WithOne(u => u.Company)
                .HasForeignKey<Company>(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // JobPosting ↔ Company (Many-to-One)
            builder.Entity<JobPosting>()
                .HasOne(j => j.Company)
                .WithMany(c => c.JobPostings)
                .HasForeignKey(j => j.CompanyID)
                .OnDelete(DeleteBehavior.NoAction);

            // JobMetric ↔ JobPosting (One-to-One)
            builder.Entity<JobMetric>()
                .HasOne(jm => jm.JobPosting)
                .WithOne(jp => jp.JobMetric)
                .HasForeignKey<JobMetric>(jm => jm.JobID)
                .OnDelete(DeleteBehavior.NoAction);

            // Application ↔ JobPosting (Many-to-One)
            builder.Entity<Application>()
                .HasOne(a => a.JobPosting)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobPostingID)
                .OnDelete(DeleteBehavior.NoAction);

            // Application ↔ Applicant (Many-to-One)
            builder.Entity<Application>()
                .HasOne(a => a.Applicant)
                .WithMany(ap => ap.Applications)
                .HasForeignKey(a => a.ApplicantID)
                .OnDelete(DeleteBehavior.NoAction);

            // Application ↔ Resume (Optional)
            builder.Entity<Application>()
                .HasOne(a => a.Resume)
                .WithMany(r => r.Applications)
                .HasForeignKey(a => a.ResumeID)
                .OnDelete(DeleteBehavior.NoAction);

            // Applicant ↔ Skill (Many-to-Many)
            builder.Entity<ApplicantSkill>()
                .HasKey(x => x.ApplicantSkillID);

            builder.Entity<ApplicantSkill>()
                .HasOne(x => x.Applicant)
                .WithMany(a => a.ApplicantSkills)
                .HasForeignKey(x => x.ApplicantID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicantSkill>()
                .HasOne(x => x.Skill)
                .WithMany(s => s.ApplicantSkills)
                .HasForeignKey(x => x.SkillID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Project>()
                .HasOne(p => p.Applicant)
                .WithMany(a => a.Projects)
                .HasForeignKey(p => p.ApplicantID)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Resume>()
            //    .HasIndex(r => new { r.ApplicantID,r.IsActive })
            //    .HasFilter("[IsActive] = 1")
            //    .IsUnique();

            // Applicant ↔ Experience (One-To-Many)
            builder.Entity<Applicant>()
                .HasMany(a => a.Experiences)
                .WithOne(ex => ex.Applicant)
                .HasForeignKey(ex => ex.ApplicantID)
                .OnDelete(DeleteBehavior.Cascade) ;
        }
    }
}
