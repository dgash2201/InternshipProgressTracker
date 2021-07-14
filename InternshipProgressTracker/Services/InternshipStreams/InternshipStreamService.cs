using AutoMapper;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Models.InternshipStreams;
using InternshipProgressTracker.Repositories.InternshipStreams;
using InternshipProgressTracker.Services.Students;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.InternshipStreams
{
    /// <summary>
    /// Logic service which works with internship streams
    /// </summary>
    public class InternshipStreamService : IInternshipStreamService
    {
        private readonly IInternshipStreamRepository _internshipStreamRepository;
        private readonly IMapper _mapper;
        private readonly IStudentService _studentService;

        public InternshipStreamService(IInternshipStreamRepository internshipStreamRepository, 
            IMapper mapper,
            IStudentService studentService)
        {
            _internshipStreamRepository = internshipStreamRepository;
            _mapper = mapper;
            _studentService = studentService;
        }

        /// <summary>
        /// Adds student to the student collection
        /// </summary>
        public async Task AddStudent(int streamId, int studentId)
        {
            var student = await _studentService.Get(studentId);
            var stream = await Get(streamId);

            stream.Students.Add(student);
            await _studentService.SetStreamId(student, streamId);
        }

        /// <summary>
        /// Gets list of internship streams
        /// </summary>
        public async Task<IEnumerable<InternshipStream>> Get()
        {
            return await _internshipStreamRepository.Get();
        }

        /// <summary>
        /// Gets internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task<InternshipStream> Get(int id)
        {
            return await _internshipStreamRepository.Get(id);
        }
        
        /// <summary>
        /// Creates internship stream from createDto
        /// </summary>
        /// <param name="createDto">Data for creation</param>
        public async Task<int> Create(CreateInternshipStreamDto createDto)
        {
            var internshipStream = _mapper.Map<CreateInternshipStreamDto, InternshipStream>(createDto);
            var id = await _internshipStreamRepository.Add(internshipStream);

            return id;
        }
        
        /// <summary>
        /// Updates data in internship stream
        /// </summary>
        /// <param name="id">Internship stream id</param>
        /// <param name="updateDto">New data</param>
        public async Task Update(int id, UpdateInternshipStreamDto updateDto)
        {
            var internshipStream = await _internshipStreamRepository.Get(id);

            _mapper.Map(updateDto, internshipStream);

            await _internshipStreamRepository.Update(internshipStream);
        }

        /// <summary>
        /// Deletes internship stream by id
        /// </summary>
        /// <param name="id">Internship stream id</param>
        public async Task Delete(int id)
        {
            await _internshipStreamRepository.Delete(id);
        }
    }
}
