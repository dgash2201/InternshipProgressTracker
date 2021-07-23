using AutoMapper;
using AutoMapper.QueryableExtensions;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlanEntries;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IReadOnlyCollection<StudyPlanEntryResponseDto>> GetAsync()
        {
            var studyPlanEntries = await _dbContext
                .StudyPlanEntries
                .ProjectTo<StudyPlanEntryResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return studyPlanEntries.AsReadOnly();
        }

        /// <summary>
        /// Gets study plan entry by id
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task<StudyPlanEntryResponseDto> GetAsync(int id)
        {
            var studyPlanEntryDto = await _dbContext
                .StudyPlanEntries
                .Where(e => e.Id == id)
                .ProjectTo<StudyPlanEntryResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (studyPlanEntryDto == null)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            return studyPlanEntryDto;
        }

        /// <summary>
        /// Creates study plan entry
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        public async Task<int> CreateAsync(StudyPlanEntryDto createDto)
        {
            var studyPlan = await _dbContext
                .StudyPlans
                .FindAsync(createDto.StudyPlanId);

            if (studyPlan == null)
            {
                throw new NotFoundException("Related study plan was not found");
            }

            var studyPlanEntry = _mapper.Map<StudyPlanEntryDto, StudyPlanEntry>(createDto);
            studyPlanEntry.StudyPlan = studyPlan;

            _dbContext.StudyPlanEntries.Add(studyPlanEntry);
            await _dbContext.SaveChangesAsync();

            return studyPlanEntry.Id;
        }

        /// <summary>
        /// Updates study plan entry data
        /// </summary>
        /// <param name="id">Id of study plan entry</param>
        /// <param name="updateDto">New data</param>
        public async Task UpdateAsync(int id, StudyPlanEntryDto updateDto)
        {
            var studyPlanEntry = await _dbContext.StudyPlanEntries.FindAsync(id);

            if (studyPlanEntry == null)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var studyPlan = await _dbContext
                .StudyPlans
                .FindAsync(updateDto.StudyPlanId);

            if (studyPlan == null)
            {
                throw new NotFoundException("Related study plan was not found");
            }

            _mapper.Map(updateDto, studyPlanEntry);
            studyPlanEntry.StudyPlan = studyPlan;

            _dbContext.Entry(studyPlanEntry).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Patches study plan
        /// </summary>
        /// <param name="id">Id of study plan entry</param>
        /// <param name="patchDocument">Json Patch operations</param>
        public async Task UpdateAsync(int id, JsonPatchDocument<StudyPlanEntryDto> patchDocument)
        {
            var studyPlanEntry = await _dbContext.StudyPlanEntries.FindAsync(id);

            if (studyPlanEntry == null)
            {
                throw new NotFoundException("Study plan entry with this id was not found");
            }

            var studyPlanEntryDto = _mapper.Map<StudyPlanEntryDto>(studyPlanEntry);
            patchDocument.ApplyTo(studyPlanEntryDto);

            var studyPlan = await _dbContext
                .StudyPlans
                .FindAsync(studyPlanEntryDto.StudyPlanId);

            if (studyPlan == null)
            {
                throw new NotFoundException("Related study plan was not found");
            }

            _mapper.Map(studyPlanEntryDto, studyPlanEntry);
            studyPlanEntry.StudyPlan = studyPlan;

            _dbContext.Entry(studyPlanEntry).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Marks study plan entry as deleted
        /// </summary>
        /// <param name="id">Id of study plan entry</param>
        public async Task SoftDeleteAsync(int id)
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
        public async Task DeleteAsync(int id)
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
