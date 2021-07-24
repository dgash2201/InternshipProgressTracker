﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
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
        /// Binds student with stream
        /// </summary>
        public async Task AddStudentAsync(int streamId, int studentId)
        {
            var stream = await _dbContext
                .InternshipStreams
                .FirstOrDefaultAsync(s => s.Id == streamId);

            if (stream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            await _studentService.SetStreamAsync(studentId, stream);
        }

        /// <summary>
        /// Gets list of internship streams
        /// </summary>
        public async Task<IReadOnlyCollection<InternshipStreamResponseDto>> GetAsync()
        {
            var internshipStreamDtos = await _dbContext
                .InternshipStreams
                .Include(s => s.Students)
                .Include(s => s.StudyPlans)
                .ThenInclude(p => p.Entries)
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
        public async Task UpdateAsync(int id, InternshipStreamResponseDto updateDto)
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
