using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;

namespace InternshipProgressTracker.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
           CreateMap<CreateInternshipStreamDto, InternshipStream>();
        }
    }
}
