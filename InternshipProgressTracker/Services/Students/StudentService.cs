using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Repositories.Students;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> Get(int id)
        {
            return await _studentRepository.Get(id);
        }

        public async Task Create(User user)
        {
            var student = new Student
            {
                Id = user.Id,
                User = user,
            };

            await _studentRepository.Add(student);
        }

        public async Task SetStreamId(Student student, int streamId)
        {
            student.Id = streamId;
            await _studentRepository.Update(student);
        }
    }
}
