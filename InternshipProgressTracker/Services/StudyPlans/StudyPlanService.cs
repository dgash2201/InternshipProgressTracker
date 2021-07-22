using AutoMapper;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.StudyPlans;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IReadOnlyCollection<StudyPlan>> GetAsync()
        {
            var studyPlans = await _dbContext
                .StudyPlans
                .Include(p => p.Entries)
                .ToListAsync();

            return studyPlans.AsReadOnly();
        }

        /// <summary>
        /// Gets study plan by id
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task<StudyPlan> GetAsync(int id)
        {
            var studyPlan = await _dbContext
                .StudyPlans
                .Include(p => p.Entries)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (studyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            return studyPlan;
        }

        /// <summary>
        /// Creates study plan
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        public async Task<int> CreateAsync(CreateStudyPlanDto createDto)
        {
            var stream = await _dbContext
                .InternshipStreams
                .FindAsync(createDto.InternshipStreamId);

            if (stream == null)
            {
                throw new NotFoundException("Internship stream with this id was not found");
            }

            var studyPlan = _mapper.Map<CreateStudyPlanDto, StudyPlan>(createDto);

            studyPlan.InternshipStream = stream;

            _dbContext.StudyPlans.Add(studyPlan);
            await _dbContext.SaveChangesAsync();

            return studyPlan.Id;
        }

        /// <summary>
        /// Updates study plan data
        /// </summary>
        /// <param name="id">Id of study plan</param>
        /// <param name="updateDto">New data</param>
        public async Task UpdateAsync(int id, UpdateStudyPlanDto updateDto)
        {
            var studyPlan = await _dbContext.StudyPlans.FindAsync(id);

            if (studyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            _mapper.Map(updateDto, studyPlan);
            _dbContext.Entry(studyPlan).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Marks study plan as deleted
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task SoftDeleteAsync(int id)
        {
            var stydyPlan = await _dbContext.StudyPlans.FindAsync(id);

            if (stydyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            stydyPlan.IsDeleted = true;
            _dbContext.Entry(stydyPlan).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes study plan
        /// </summary>
        /// <param name="id">Id of study plan</param>
        public async Task DeleteAsync(int id)
        {
            var studyPlan = _dbContext.FindTracked<StudyPlan>(id);

            if (studyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            _dbContext.Remove(_dbContext.FindTracked<StudyPlan>(id));
            await _dbContext.SaveChangesAsync();
        }
    }
}
