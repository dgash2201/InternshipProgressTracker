using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using InternshipProgressTracker.Entities;

namespace InternshipProgressTracker
{
    public class InternshipProgressTrackerDbContext : DbContext
    {
        public InternshipProgressTrackerDbContext([NotNull] DbContextOptions options) 
            : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudyPlanEntry> StudyPlanEntries { get; set; }
        public DbSet<StudentStudyPlanProgress> StudentStudyPlanProgresses { get; set; }
        public DbSet<InternshipStream> InternshipStreams { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<StudentStudyPlanProgress>()
                .HasKey(e => new { e.StudentId, e.StudyPlanEntryId });

            builder
                .Entity<Student>()
                .HasMany(e => e.StudyPlanProgresses)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<StudyPlanEntry>()
                .HasMany(e => e.StudentsProgresses)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Mentor>()
                .HasMany(e => e.StudentStudyPlanProgresses)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
