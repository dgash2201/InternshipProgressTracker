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

        public async Task Create(User user)
        {
            var student = new Student
            {
                Id = user.Id,
                User = user,
            };

            await _studentRepository.Add(student);
        }
    }
}
