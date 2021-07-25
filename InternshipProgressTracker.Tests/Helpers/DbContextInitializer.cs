using InternshipProgressTracker.Database;
using Microsoft.EntityFrameworkCore;

namespace InternshipProgressTracker.Tests.Helpers
{
    public static class DbContextInitializer
    {
        public static InternshipProgressTrackerDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<InternshipProgressTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: "InternshipProgressTrackerDB")
            .Options;

            return new InternshipProgressTrackerDbContext(options);
        }

        public static void DisposeDbContext(this InternshipProgressTrackerDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
    }
}
