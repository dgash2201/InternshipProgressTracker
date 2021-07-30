using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
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

            CreateMap<User, UserResponseDto>()
                .ForMember(dto => dto.Avatar, 
                    options => options.MapFrom(entity => CreateAvatar(entity.Photo, entity.PhotoType)));
        }

        private FileContentResult CreateAvatar(byte[] photo, string type)
        {
            if (photo == null)
            {
                return null;
            }

            return new FileContentResult(photo, type);
        }
    }
}
