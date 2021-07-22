using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Models.StudyPlans;

namespace InternshipProgressTracker.Mapper
{        
    /// <summary>
    /// Contains settings for AutoMapper
    /// </summary>
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            CreateMap<CreateInternshipStreamDto, InternshipStream>();
            CreateMap<StudyPlanDto, StudyPlan>().ReverseMap();
            CreateMap<StudyPlanEntryDto, StudyPlanEntry>().ReverseMap();
        }
    }
}
