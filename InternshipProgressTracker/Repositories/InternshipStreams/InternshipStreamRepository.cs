using InternshipProgressTracker.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Repositories.InternshipStreams
{
    public class InternshipStreamRepository : IInternshipStreamRepository
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;

        public InternshipStreamRepository(InternshipProgressTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Add(InternshipStream internshipStream)
        {
            _dbContext.InternshipStreams.Add(internshipStream);
            await _dbContext.SaveChangesAsync();

            return internshipStream.Id;
        }

        public async Task Delete(int id)
        {
            var toRemove = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            _dbContext.Remove(toRemove);

            await _dbContext.SaveChangesAsync();
        }
    }
}
