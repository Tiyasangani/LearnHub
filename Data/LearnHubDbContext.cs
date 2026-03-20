using LearnHub.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Data
{
    public class LearnHubDbContext : DbContext
    {
        public LearnHubDbContext(DbContextOptions<LearnHubDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
        public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
        public DbSet<Certificate> Certificates => Set<Certificate>();
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique email constraint
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Enrollment relationships
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User).WithMany(u => u.Enrollments).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course).WithMany(c => c.Enrollments).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);

            // QuizQuestion relationship
            modelBuilder.Entity<QuizQuestion>()
                .HasOne(q => q.Course).WithMany(c => c.QuizQuestions).HasForeignKey(q => q.CourseId).OnDelete(DeleteBehavior.Cascade);

            // QuizAttempt relationships
            modelBuilder.Entity<QuizAttempt>()
                .HasOne(a => a.User).WithMany(u => u.QuizAttempts).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<QuizAttempt>()
                .HasOne(a => a.Course).WithMany().HasForeignKey(a => a.CourseId).OnDelete(DeleteBehavior.Cascade);

            // Certificate relationships
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Course).WithMany().HasForeignKey(c => c.CourseId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
