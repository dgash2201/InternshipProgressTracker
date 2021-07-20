using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Models.StudyPlans;

namespace InternshipProgressTracker.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateInternshipStreamDto, InternshipStream>();
            CreateMap<CreateStudyPlanDto, StudyPlan>();
            CreateMap<CreateStudyPlanEntryDto, StudyPlanEntry>();
        }
    }
}
