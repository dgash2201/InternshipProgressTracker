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
    public class StudyPlanService : IStudyPlanService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;
        private readonly IMapper _mapper; 

        public StudyPlanService(InternshipProgressTrackerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<StudyPlan>> Get()
        {
            var studyPlans = await _dbContext
                .StudyPlans
                .ToListAsync();

            return studyPlans.AsReadOnly();
        }

        public async Task<StudyPlan> Get(int id)
        {
            var studyPlan = await _dbContext.StudyPlans.FindAsync(id);

            if (studyPlan == null)
            {
                throw new NotFoundException("Study plan with this id was not found");
            }

            return studyPlan;
        }

        public async Task<int> Create(CreateStudyPlanDto createDto)
        {
            var studyPlan = _mapper.Map<CreateStudyPlanDto, StudyPlan>(createDto);

            _dbContext.StudyPlans.Add(studyPlan);
            await _dbContext.SaveChangesAsync();

            return studyPlan.Id;
        }

        public async Task Update(int id, UpdateStudyPlanDto updateDto)
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

        public async Task SoftDelete(int id)
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

        public async Task Delete(int id)
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
