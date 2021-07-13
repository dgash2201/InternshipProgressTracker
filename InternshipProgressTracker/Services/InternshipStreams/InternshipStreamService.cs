using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Repositories.InternshipStreams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    public class InternshipStreamService : IInternshipStreamService
    {
        private readonly IInternshipStreamRepository _internshipStreamRepository;
        private readonly IMapper _mapper;

        public InternshipStreamService(IInternshipStreamRepository internshipStreamRepository, IMapper mapper)
        {
            _internshipStreamRepository = internshipStreamRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InternshipStream>> Get()
        {
            return await _internshipStreamRepository.Get();
        }

        public async Task<InternshipStream> Get(int id)
        {
            return await _internshipStreamRepository.Get(id);
        }

        public async Task<int> Create(CreateInternshipStreamDto createDto)
        {
            var internshipStream = _mapper.Map<CreateInternshipStreamDto, InternshipStream>(createDto);
            var id = await _internshipStreamRepository.Add(internshipStream);

            return id;
        }
        
        public async Task Update(int id, UpdateInternshipStreamDto updateDto)
        {
            var internshipStream = await _internshipStreamRepository.Get(id);

            _mapper.Map(updateDto, internshipStream);

            await _internshipStreamRepository.Update(internshipStream);
        }

        public async Task Delete(int id)
        {
            await _internshipStreamRepository.Delete(id);
        }
    }
}
