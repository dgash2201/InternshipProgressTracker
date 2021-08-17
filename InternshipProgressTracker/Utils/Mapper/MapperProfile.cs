using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Models.StudentStudyPlanProgresses;
using InternshipProgressTracker.Models.StudyPlanEntries;
using InternshipProgressTracker.Models.StudyPlans;
using InternshipProgressTracker.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InternshipProgressTracker.Utils.Mapper
{        
    /// <summary>
    /// Contains settings for AutoMapper
    /// </summary>
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<InternshipStreamDto, InternshipStream>().ReverseMap();
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

            CreateMap<User, UserResponseDto>();

            CreateMap<Student, StudentResponseDto>()
                .ForMember(dto => dto.StudyPlanProgresses,
                    options => options.MapFrom(entity => entity.StudyPlanProgresses.ToList()));

            CreateMap<Mentor, MentorResponseDto>()
                .ForMember(dto => dto.StudentProgresses,
                    options => options.MapFrom(entity => entity.StudentStudyPlanProgresses.ToList()));

            CreateMap<StudentStudyPlanProgress, StudentProgressResponseDto>();
        }
    }
}
