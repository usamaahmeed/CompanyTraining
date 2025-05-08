using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CompanyTraining.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationCompany>
    {
        public DbSet<ApplicationCompany> ApplicationCompanies { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().HasIndex(e => new { e.Email }).IsUnique();
            builder.Entity<Course>().HasOne(e => e.Quiz).WithOne(u => u.Course).HasForeignKey<Quiz>(e => e.CourseId);
            builder.Entity<Certificate>().HasOne(e => e.UserCourse).WithOne(e => e.Certificate).HasForeignKey<UserCourse>(e => e.CertificateId);
          
            builder.Entity<UserCourse>()
    .HasOne(uc => uc.Course)
    .WithMany(c => c.UserCourses)
    .HasForeignKey(uc => uc.CourseId)
    .OnDelete(DeleteBehavior.NoAction); // Disable cascade on CourseId

            builder.Entity<UserCourse>()
                .HasOne(uc => uc.ApplicationUser)
                .WithMany(u => u.UserCourses)
                .HasForeignKey(uc => uc.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction); // Disable cascade on ApplicationUserId

            builder.Entity<UserQuizAttempt>()
                .HasOne(q => q.Quiz)
                .WithMany(uq=>uq.UserQuizAttempts)
                .HasForeignKey(uq=>uq.QuizId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserQuizAttempt>()
               .HasOne(q => q.ApplicationUser)
               .WithMany(uq => uq.UserQuizAttempts)
               .HasForeignKey(uq => uq.ApplicationUserId)
               .OnDelete(DeleteBehavior.NoAction);

        }

    }

}
