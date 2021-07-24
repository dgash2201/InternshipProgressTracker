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
            CreateMap<InternshipStreamDto, InternshipStream>();
            CreateMap<StudyPlanDto, StudyPlan>().ReverseMap();
            CreateMap<StudyPlanEntryDto, StudyPlanEntry>().ReverseMap();

            CreateMap<InternshipStream, InternshipStreamResponseDto>()
                .ForMember(dto => dto.StudyPlans,
                    options => options.MapFrom(entity => entity.StudyPlans.ToList()));

            CreateMap<StudyPlan, StudyPlanResponseDto>()
                .ForMember(dto => dto.Entries, 
                    options => options.MapFrom(entity => entity.Entries.ToList()));

            CreateMap<StudyPlanEntry, StudyPlanEntryResponseDto>()
                .ForMember(dto => dto.StudentProgresses, 
                    options => options.MapFrom(entity => entity.StudentsProgresses.ToList()));
        }
    }
}
