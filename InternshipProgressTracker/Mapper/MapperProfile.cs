using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Models.StudyPlans;
using System.Linq;

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
            CreateMap<StudyPlan, StudyPlanResponseDto>()
                .ForMember(dest => dest.Entries, 
                    options => options.MapFrom(source => source.Entries.ToList().AsReadOnly()));
            CreateMap<StudyPlanEntry, StudyPlanEntryResponseDto>()
                .ForMember(dest => dest.StudentProgresses, 
                    options => options.MapFrom(source => source.StudentsProgresses.ToList().AsReadOnly()));
        }
    }
}
