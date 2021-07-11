using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Repositories.InternshipStreams;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    public class InternshipStreamService : IInternshipStreamService
    {
        private readonly IInternshipStreamRepository _internshipStreamRepository;

        public InternshipStreamService(IInternshipStreamRepository internshipStreamRepository)
        {
            _internshipStreamRepository = internshipStreamRepository;
        }

        public async Task<int> Create(CreateInternshipStreamDto createDto)
        {
            var internshipStream = new InternshipStream
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Status = createDto.Status
            };

            var id = await _internshipStreamRepository.Add(internshipStream);

            return id;
        }
    }
}
