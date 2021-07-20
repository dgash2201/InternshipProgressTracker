using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Models.StudyPlans;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.StudyPlanEntries
{
    /// <summary>
    /// Service for working with study plan entries
    /// </summary>
    public class StudyPlanEntryService : IStudyPlanEntryService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;
        private readonly IMapper _mapper; 

        public StudyPlanEntryService(InternshipProgressTrackerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all study plan entries
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<StudyPlanEntry>> Get()
        {
            var studyPlanEntries = await _dbContext
                .StudyPlanEntries
                .ToListAsync();

            return studyPlanEntries.AsReadOnly();
        }

        /// <summary>
        /// Gets study plan entry by id
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task<StudyPlanEntry> Get(int id)
        {
            var studyPlanEntry = await _dbContext.StudyPlanEntries.FindAsync(id);

            if (studyPlanEntry == null)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            return studyPlanEntry;
        }

        /// <summary>
        /// Creates study plan entry
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        public async Task<int> Create(CreateStudyPlanEntryDto createDto)
        {
            var studyPlanEntry = _mapper.Map<CreateStudyPlanEntryDto, StudyPlanEntry>(createDto);

            _dbContext.StudyPlanEntries.Add(studyPlanEntry);
            await _dbContext.SaveChangesAsync();

            return studyPlanEntry.Id;
        }

        /// <summary>
        /// Updates study plan entry data
        /// </summary>
        /// <param name="id">Id of study plan entry</param>
        /// <param name="updateDto">New data</param>
        public async Task Update(int id, UpdateStudyPlanEntryDto updateDto)
        {
            var studyPlanEntry = await _dbContext.StudyPlanEntries.FindAsync(id);

            if (studyPlanEntry == null)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            _mapper.Map(updateDto, studyPlanEntry);
            _dbContext.Entry(studyPlanEntry).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Marks study plan entry as deleted
        /// </summary>
        /// <param name="id">Id of study plan entry</param>
        public async Task SoftDelete(int id)
        {
            var stydyPlanEntry = await _dbContext.StudyPlanEntries.FindAsync(id);

            if (stydyPlanEntry == null)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            stydyPlanEntry.IsDeleted = true;
            _dbContext.Entry(stydyPlanEntry).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes study plan entry
        /// </summary>
        /// <param name="id">Id of study plan entry</param>
        public async Task Delete(int id)
        {
            var studyPlanEntry = _dbContext.FindTracked<StudyPlan>(id);

            if (studyPlanEntry == null)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            _dbContext.Remove(_dbContext.FindTracked<StudyPlanEntry>(id));
            await _dbContext.SaveChangesAsync();
        }
    }
}
