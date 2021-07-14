using InternshipProgressTracker.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Repositories.InternshipStreams
{
    /// <summary>
    /// Works with database
    /// </summary>
    public class InternshipStreamRepository : IInternshipStreamRepository
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;

        public InternshipStreamRepository(InternshipProgressTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public async Task<IEnumerable<InternshipStream>> Get()
        {
            return _dbContext
                .InternshipStreams
                .AsEnumerable();
        }
        
        /// <summary>
        /// Gets internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task<InternshipStream> Get(int id)
        {
            return await _dbContext
                .InternshipStreams
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds internship stream to the database
        /// </summary>
        /// <param name="internshipStream">Internship stream entity</param>
        public async Task<int> Add(InternshipStream internshipStream)
        {
            _dbContext.InternshipStreams.Add(internshipStream);
            await _dbContext.SaveChangesAsync();

            return internshipStream.Id;
        }

        /// <summary>
        /// Saves updated internship stream in the database
        /// </summary>
        /// <param name="internshipStream">Updated internship stream entity</param>
        public async Task Update(InternshipStream internshipStream)
        {
            _dbContext.Entry(internshipStream).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
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
