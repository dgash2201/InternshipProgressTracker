using AutoMapper;
using AutoMapper.QueryableExtensions;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Services.Extensions;
using InternshipProgressTracker.Services.Students;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
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

        public InternshipStreamService(InternshipProgressTrackerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Binds mentor with stream
        /// </summary>
        public async Task AddMentorAsync(int streamId, int mentorId)
        {
            var stream = await _dbContext
                .InternshipStreams
                .FindAsync(streamId);

            var mentor = await _dbContext
                .Mentors
                .FindAsync(mentorId);

            if (stream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            if (mentor == null)
            {
                throw new NotFoundException("Mentor with this id was not found");
            }

            if (stream.Mentors == null)
            {
                stream.Mentors = new Collection<Mentor>();
            }

            stream.Mentors.Add(mentor);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Binds student with stream
        /// </summary>
        public async Task AddStudentAsync(int streamId, int studentId)
        {
            var stream = await _dbContext
                .InternshipStreams
                .FindAsync(streamId);

            var student = await _dbContext
                .Students
                .FindAsync(studentId);

            if (stream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            if (student == null)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            if (stream.Students == null)
            {
                stream.Students = new Collection<Student>();
            }

            stream.Students.Add(student);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all internship streams
        /// </summary>
        public async Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetWithSoftDeletedAsync()
        {
            var internshipStreamDtos = await _dbContext
                .InternshipStreams
                .Include(s => s.Students)
                .Include(s => s.Mentors)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .IgnoreQueryFilters()
                .ProjectTo<InternshipStreamResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return internshipStreamDtos.AsReadOnly();
        }

        /// <summary>
        /// Gets list of internship streams
        /// </summary>
        public async Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetAsync(int? studentId, int? mentorId)
        {
            var filters = new List<Expression<Func<InternshipStream, bool>>>();

            if (studentId != null)
            {
                filters.Add(stream => stream.Students.Any(student => student.Id == studentId));
            }

            if (mentorId != null)
            {
                filters.Add(stream => stream.Mentors.Any(mentor => mentor.Id == mentorId));
            }

            var internshipStreamDtos = await _dbContext
                .InternshipStreams
                .Include(s => s.Students)
                .Include(s => s.Mentors)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .ApplyFilters(filters)
                .ProjectTo<InternshipStreamResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return internshipStreamDtos.AsReadOnly();
        }

        /// <summary>
        /// Gets internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task<InternshipStreamResponseDto> GetAsync(int id)
        {
            var internshipStreamDto = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == id)
                .Include(s => s.Students)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .ProjectTo<InternshipStreamResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (internshipStreamDto == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            return internshipStreamDto;
        }
        
        /// <summary>
        /// Creates internship stream from createDto
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        public async Task<int> CreateAsync(InternshipStreamDto createDto)
        {
            var internshipStream = _mapper.Map<InternshipStreamDto, InternshipStream>(createDto);
            _dbContext.InternshipStreams.Add(internshipStream);
            await _dbContext.SaveChangesAsync();

            return internshipStream.Id;
        }
        
        /// <summary>
        /// Updates data in internship stream
        /// </summary>
        /// <param name="id">Internship stream id</param>
        /// <param name="updateDto">New data</param>
        public async Task UpdateAsync(int id, InternshipStreamDto updateDto)
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
        /// Patches internship stream
        /// </summary>
        /// <param name="id">Id of internship stream</param>
        /// <param name="patchDocument">Json Patch operations</param>
        public async Task UpdateAsync(int id, JsonPatchDocument<InternshipStreamDto> patchDocument)
        {
            var internshipStream = await _dbContext.InternshipStreams.FindAsync(id);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            var internshipStreamDto = _mapper.Map<InternshipStreamDto>(internshipStream);
            patchDocument.ApplyTo(internshipStreamDto);

            _mapper.Map(internshipStreamDto, internshipStream);

            _dbContext.Entry(internshipStream).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Marks the entity as deleted
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task SoftDeleteAsync(int id)
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
        public async Task DeleteAsync(int id)
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
