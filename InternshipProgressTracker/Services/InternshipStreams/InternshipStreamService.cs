using AutoMapper;
using AutoMapper.QueryableExtensions;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Services.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
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
        public async Task<InternshipStreamResponseDto> AddMentorAsync(
            int streamId, int mentorId, CancellationToken cancellationToken = default)
        {
            var stream = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == streamId)
                .Include(s => s.Students)
                .Include(s => s.Mentors)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .FirstOrDefaultAsync(cancellationToken);

            var mentor = await _dbContext
                .Mentors
                .FindAsync(new object[] { mentorId }, cancellationToken);

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
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InternshipStreamResponseDto>(stream);
        }

        /// <summary>
        /// Binds student with stream
        /// </summary>
        public async Task<InternshipStreamResponseDto> AddStudentAsync(
            int streamId, int studentId, CancellationToken cancellationToken = default)
        {
            var stream = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == streamId)
                .Include(s => s.Students)
                .Include(s => s.Mentors)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .FirstOrDefaultAsync(cancellationToken);

            var student = await _dbContext
                .Students
                .FindAsync(new object[] { studentId }, cancellationToken);

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
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InternshipStreamResponseDto>(stream);
        }

        /// <summary>
        /// Remove mentor from internship stream
        /// </summary>
        public async Task<InternshipStreamResponseDto> RemoveMentorAsync(
            int streamId, int mentorId, CancellationToken cancellationToken = default)
        {
            var stream = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == streamId)
                .Include(s => s.Students)
                .Include(s => s.Mentors)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .FirstOrDefaultAsync(cancellationToken);

            var mentor = await _dbContext
                .Mentors
                .FindAsync(new object[] { mentorId }, cancellationToken);

            if (stream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            if (mentor == null)
            {
                throw new NotFoundException("Mentor with this id was not found");
            }

            stream.Mentors.Remove(mentor);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InternshipStreamResponseDto>(stream);
        }

        /// <summary>
        /// Remove student from internship stream
        /// </summary>
        public async Task<InternshipStreamResponseDto> RemoveStudentAsync(
            int streamId, int studentId, CancellationToken cancellationToken = default)
        {
            var stream = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == streamId)
                .Include(s => s.Students)
                .Include(s => s.Mentors)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .FirstOrDefaultAsync(cancellationToken);

            var student = await _dbContext
                .Students
                .FindAsync(new object[] { studentId }, cancellationToken);

            if (stream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            if (student == null)
            {
                throw new NotFoundException("Student with this id was not found");
            }

            stream.Students.Remove(student);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InternshipStreamResponseDto>(stream);
        }

        /// <summary>
        /// Gets all internship streams
        /// </summary>
        public async Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetWithSoftDeletedAsync(
            CancellationToken cancellationToken = default)
        {
            var internshipStreamDtos = await _dbContext
                .InternshipStreams
                .Include(s => s.Students)
                .Include(s => s.Mentors)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .IgnoreQueryFilters()
                .ProjectTo<InternshipStreamResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return internshipStreamDtos.AsReadOnly();
        }

        /// <summary>
        /// Gets list of internship streams
        /// </summary>
        public async Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetAsync(int? studentId, int? mentorId, CancellationToken cancellationToken = default)
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
                .ToListAsync(cancellationToken);

            return internshipStreamDtos.AsReadOnly();
        }

        /// <summary>
        /// Gets internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task<InternshipStreamResponseDto> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var internshipStreamDto = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == id)
                .Include(s => s.Students)
                .Include(s => s.Mentors)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .ProjectTo<InternshipStreamResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

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
        public async Task<InternshipStreamResponseDto> CreateAsync(InternshipStreamDto createDto, CancellationToken cancellationToken = default)
        {
            var internshipStream = _mapper.Map<InternshipStreamDto, InternshipStream>(createDto);
            _dbContext.InternshipStreams.Add(internshipStream);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InternshipStreamResponseDto>(internshipStream);
        }

        /// <summary>
        /// Updates data in internship stream
        /// </summary>
        /// <param name="id">Internship stream id</param>
        /// <param name="updateDto">New data</param>
        public async Task<InternshipStreamResponseDto> UpdateAsync(int id, InternshipStreamDto updateDto, CancellationToken cancellationToken = default)
        {
            var internshipStream = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == id)
                .Include(s => s.Students)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .FirstOrDefaultAsync(cancellationToken);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            _mapper.Map(updateDto, internshipStream);
            _dbContext.Entry(internshipStream).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InternshipStreamResponseDto>(internshipStream);
        }

        /// <summary>
        /// Patches internship stream
        /// </summary>
        /// <param name="id">Id of internship stream</param>
        /// <param name="patchDocument">Json Patch operations</param>
        public async Task<InternshipStreamResponseDto> UpdateAsync(int id, JsonPatchDocument<InternshipStreamDto> patchDocument, CancellationToken cancellationToken = default)
        {
            var internshipStream = await _dbContext
                .InternshipStreams
                .Where(e => e.Id == id)
                .Include(s => s.Students)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
                .FirstOrDefaultAsync(cancellationToken);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            var internshipStreamDto = _mapper.Map<InternshipStreamDto>(internshipStream);
            patchDocument.ApplyTo(internshipStreamDto);

            _mapper.Map(internshipStreamDto, internshipStream);

            _dbContext.Entry(internshipStream).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<InternshipStreamResponseDto>(internshipStream);
        }

        /// <summary>
        /// Marks the entity as deleted
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var internshipStream = await _dbContext.InternshipStreams.FindAsync(new object[] { id }, cancellationToken);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            internshipStream.IsDeleted = true;
            _dbContext.Entry(internshipStream).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var internshipStream = await _dbContext.InternshipStreams.FindAsync(new object[] { id }, cancellationToken);

            if (internshipStream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            _dbContext.Remove(internshipStream);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
