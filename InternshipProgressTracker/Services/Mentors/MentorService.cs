using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Mentors
{
    /// <summary>
    /// Logic service which works with students
    /// </summary>
    public class MentorService : IMentorService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;

        public MentorService(InternshipProgressTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a student based on the user
        /// </summary>
        public async Task CreateAsync(User user)
        {
            var mentor = new Mentor
            {
                Id = user.Id,
                User = user
            };

            _dbContext.Mentors.Add(mentor);
            await _dbContext.SaveChangesAsync();
        }
    }
}
