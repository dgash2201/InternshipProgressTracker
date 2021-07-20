using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using InternshipProgressTracker.Entities;

namespace InternshipProgressTracker.Database
{
    /// <summary>
    /// Class for working with InternshipProgressTracker database
    /// </summary>
    public class InternshipProgressTrackerDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public InternshipProgressTrackerDbContext([NotNull] DbContextOptions options) 
            : base(options) {}

        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudyPlanEntry> StudyPlanEntries { get; set; }
        public DbSet<StudentStudyPlanProgress> StudentStudyPlanProgresses { get; set; }
        public DbSet<InternshipStream> InternshipStreams { get; set; }

        /// <summary>
        /// Method for configuring relationships between entities by Fluent API
        /// </summary>
        /// <param name="builder">Builder to construct a model for a context</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
                .HasForeignKey(s => s.GradingMentorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<User>()
                .HasKey(e => e.Id);

            builder
                .Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId);

            builder
                .Entity<User>()
                .HasOne(u => u.Mentor)
                .WithOne(m => m.User)
                .HasForeignKey<Mentor>(m => m.UserId);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                //other automated configurations left out
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }
    }
}
