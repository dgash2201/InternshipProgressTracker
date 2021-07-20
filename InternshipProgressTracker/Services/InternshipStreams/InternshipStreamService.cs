using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Services.Students;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    /// <summary>
    /// Logic service which works with internship streams
    /// </summary>
    public class InternshipStreamService : IInternshipStreamService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStudentService _studentService;

        public InternshipStreamService(InternshipProgressTrackerDbContext dbContext, 
            IMapper mapper,
            IStudentService studentService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _studentService = studentService;
        }

        /// <summary>
        /// Binds student with stream
        /// </summary>
        public async Task AddStudent(int streamId, int studentId)
        {
            var stream = await _dbContext
                .InternshipStreams
                .FirstOrDefaultAsync(s => s.Id == streamId);

            if (stream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            await _studentService.SetStream(studentId, stream);
        }

        /// <summary>
        /// Gets list of internship streams
        /// </summary>
        public async Task<IReadOnlyCollection<InternshipStream>> Get()
        {
            var internshipStreams = await _dbContext
                .InternshipStreams
                .Include(s => s.Students)
                .ToListAsync();

            return internshipStreams
                .AsReadOnly();
        }

        /// <summary>
        /// Gets internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task<InternshipStream> Get(int id)
        {
            var internshipStream =  await _dbContext
                .InternshipStreams
                .Include(s => s.Students)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            return internshipStream;
        }
        
        /// <summary>
        /// Creates internship stream from createDto
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        public async Task<int> Create(CreateInternshipStreamDto createDto)
        {
            var internshipStream = _mapper.Map<CreateInternshipStreamDto, InternshipStream>(createDto);
            _dbContext.InternshipStreams.Add(internshipStream);
            await _dbContext.SaveChangesAsync();

            return internshipStream.Id;
        }
        
        /// <summary>
        /// Updates data in internship stream
        /// </summary>
        /// <param name="id">Internship stream id</param>
        /// <param name="updateDto">New data</param>
        public async Task Update(int id, UpdateInternshipStreamDto updateDto)
        {
            var internshipStream = await _dbContext.InternshipStreams.FindAsync(id);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            _mapper.Map(updateDto, internshipStream);
            _dbContext.Entry(internshipStream).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Marks the entity as deleted
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task SoftDelete(int id)
        {
            var internshipStream = await _dbContext.InternshipStreams.FindAsync(id);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            internshipStream.IsDeleted = true;
            _dbContext.Entry(internshipStream).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task Delete(int id)
        {
            var internshipStream = _dbContext.FindTracked<InternshipStream>(id);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            _dbContext.Remove(_dbContext.FindTracked<InternshipStream>(id));
            await _dbContext.SaveChangesAsync();
        }
    }
}
