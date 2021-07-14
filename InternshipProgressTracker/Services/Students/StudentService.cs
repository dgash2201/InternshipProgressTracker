using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Repositories.Students;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Students
{
    /// <summary>
    /// Logic service which works with students
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// Gets a student by id
        /// </summary>
        /// <param name="id">Student id</param>
        public async Task<Student> Get(int id)
        {
            return await _studentRepository.Get(id);
        }

        /// <summary>
        /// Creates a student based on the user
        /// </summary>
        /// <param name="user">User entity</param>
        public async Task Create(User user)
        {
            var student = new Student
            {
                Id = user.Id,
                User = user,
            };

            await _studentRepository.Add(student);
        }

        /// <summary>
        /// Binds student with internship stream
        /// </summary>
        public async Task SetStreamId(Student student, int streamId)
        {
            student.Id = streamId;
            await _studentRepository.Update(student);
        }
    }
}
