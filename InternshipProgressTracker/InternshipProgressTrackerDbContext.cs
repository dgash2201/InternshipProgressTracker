using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace InternshipProgressTracker
{
    public class InternshipProgressTrackerDbContext : DbContext
    {
        public InternshipProgressTrackerDbContext([NotNull] DbContextOptions options) : base(options) {}
    }
}
