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
    public class InternshipProgressTrackerDbContext : IdentityDbContext<User, Role, int>
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
                .HasKey(p => new { p.StudentId, p.StudyPlanEntryId });

            builder
                .Entity<Student>()
                .HasMany(s => s.StudyPlanProgresses)
                .WithOne()
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<StudyPlanEntry>()
                .HasMany(e => e.StudentsProgresses)
                .WithOne()
                .HasForeignKey(p => p.StudyPlanEntryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Mentor>()
                .HasMany(m => m.StudentStudyPlanProgresses)
                .WithOne()
                .HasForeignKey(s => s.GradingMentorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<User>()
                .HasKey(u => u.Id);

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

            builder
                .Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<IdentityUserRole<int>>(
                    userRole => userRole.HasOne<Role>()
                        .WithMany()
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired(),
                    userRole => userRole.HasOne<User>()
                        .WithMany()
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired());

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }
    }
}
