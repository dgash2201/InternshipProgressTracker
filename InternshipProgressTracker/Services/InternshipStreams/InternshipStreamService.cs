using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Services.Students;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        /// Adds student to the student collection
        /// </summary>
        public async Task AddStudent(int streamId, int studentId)
        {
            var student = await _studentService.Get(studentId);
            await _studentService.SetStreamId(student, streamId);
        }

        /// <summary>
        /// Gets list of internship streams
        /// </summary>
        public async Task<IReadOnlyCollection<InternshipStream>> Get()
        {
            var internshipStreams = await _dbContext
                .InternshipStreams
                .ToListAsync();

            var students = await _studentService.Get();

            return internshipStreams
                .Join(students,
                    stream => stream.Id,
                    student => student.InternshipStreamId,
                    (stream, student) =>
                    {
                        stream.Students.Add(student);
                        return stream;
                    })
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Gets internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task<InternshipStream> Get(int id)
        {
            var internshipStream = await _dbContext
                .InternshipStreams
                .FirstOrDefaultAsync(e => e.Id == id);

            var students = await _studentService.Get();

            internshipStream.Students = students
                .Where(s => s.InternshipStreamId == id)
                .ToList();

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
            var internshipStream = await Get(id);
            _mapper.Map(updateDto, internshipStream);
            _dbContext.Entry(internshipStream).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task Delete(int id)
        {
            _dbContext.Remove(_dbContext.FindTracked<InternshipStream>(id) ?? new InternshipStream { Id = id });
            await _dbContext.SaveChangesAsync();
        }
    }
}
