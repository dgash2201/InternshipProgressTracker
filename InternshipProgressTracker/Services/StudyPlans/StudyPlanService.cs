using AutoMapper;
using AutoMapper.QueryableExtensions;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlans;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.StudyPlans
{
    /// <summary>
    /// Service for working with study plans
    /// </summary>
    public class StudyPlanService : IStudyPlanService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;
        private readonly IMapper _mapper; 

        public StudyPlanService(InternshipProgressTrackerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all study plans
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<StudyPlanResponseDto>> GetWithSoftDeletedAsync(CancellationToken cancellationToken = default)
        {
            var studyPlanDtos = await _dbContext
                .StudyPlans
                .Include(p => p.Entries)
                .IgnoreQueryFilters()
                .ProjectTo<StudyPlanResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return studyPlanDtos.AsReadOnly();
        }

        /// <summary>
        /// Gets list of study plans
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<StudyPlanResponseDto>> GetAsync(CancellationToken cancellationToken = default)
        {
            var studyPlanDtos = await _dbContext
                .StudyPlans
                .Include(p => p.Entries)
                .ProjectTo<StudyPlanResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return studyPlanDtos.AsReadOnly();
        }

        /// <summary>
        /// Gets study plan by id
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task<StudyPlanResponseDto> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var studyPlanDto = await _dbContext
                .StudyPlans
                .Where(p => p.Id == id)
                .Include(p => p.Entries)
                .ProjectTo<StudyPlanResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (studyPlanDto == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            return studyPlanDto;
        }

        /// <summary>
        /// Creates study plan
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        public async Task<StudyPlanResponseDto> CreateAsync(StudyPlanDto createDto, CancellationToken cancellationToken = default)
        {
            var stream = await _dbContext
                .InternshipStreams
                .FindAsync(new object[] { createDto.InternshipStreamId }, cancellationToken);

            if (stream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            var studyPlan = _mapper.Map<StudyPlanDto, StudyPlan>(createDto);

            studyPlan.InternshipStream = stream;

            _dbContext.StudyPlans.Add(studyPlan);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudyPlanResponseDto>(studyPlan);
        }

        /// <summary>
        /// Updates study plan data
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <param name="updateDto">New data</param>
        public async Task<StudyPlanResponseDto> UpdateAsync(int id, StudyPlanDto updateDto, CancellationToken cancellationToken = default)
        {
            var studyPlan = await _dbContext
                .StudyPlans
                .Where(p => p.Id == id)
                .Include(p => p.Entries)
                .FirstOrDefaultAsync(cancellationToken);

            if (studyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            var stream = await _dbContext
                .InternshipStreams
                .FindAsync(new object[] { updateDto.InternshipStreamId }, cancellationToken);

            if (stream == null)
            {
                throw new NotFoundException("Related internship stream was not found");
            }

            _mapper.Map(updateDto, studyPlan);
            studyPlan.InternshipStream = stream;

            _dbContext.Entry(studyPlan).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudyPlanResponseDto>(studyPlan);
        }

        /// <summary>
        /// Patches study plan
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <param name="patchDocument">Json Patch operations</param>
        public async Task<StudyPlanResponseDto> UpdateAsync(int id, JsonPatchDocument<StudyPlanDto> patchDocument, CancellationToken cancellationToken = default)
        {
            var studyPlan = await _dbContext
                .StudyPlans
                .Where(p => p.Id == id)
                .Include(p => p.Entries)
                .FirstOrDefaultAsync(cancellationToken);

            if (studyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            var studyPlanDto = _mapper.Map<StudyPlanDto>(studyPlan);
            patchDocument.ApplyTo(studyPlanDto);

            var stream = await _dbContext
                .InternshipStreams
                .FindAsync(new object[] { studyPlanDto.InternshipStreamId }, cancellationToken);

            if (stream == null)
            {
                throw new NotFoundException("Related internship stream was not found");
            }

            _mapper.Map(studyPlanDto, studyPlan);
            studyPlan.InternshipStream = stream;

            _dbContext.Entry(studyPlan).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<StudyPlanResponseDto>(studyPlan);
        }

        /// <summary>
        /// Marks study plan as deleted
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var stydyPlan = await _dbContext.StudyPlans.FindAsync(new object[] { id }, cancellationToken);

            if (stydyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            stydyPlan.IsDeleted = true;
            _dbContext.Entry(stydyPlan).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes study plan
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var studyPlan = await _dbContext.StudyPlans.FindAsync(new object[] { id }, cancellationToken);

            if (studyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            _dbContext.Remove(studyPlan);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
