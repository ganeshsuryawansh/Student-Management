using StudentManagementSystem.Models.DTOs;
using StudentManagementSystem.Repositories;
using StudentManagementSystem.Models;
using StudentManagementSystem.Middleware;

namespace StudentManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly ILogger _logger;


        public StudentService(IStudentRepository repository, ILogger<StudentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<StudentDto> AddStudentAsync(CreateStudentDto dto)
        {

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age,
                Course = dto.Course,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(student);
            _logger.LogInformation("Created student {StudentId} - {Email}", created.Id, created.Email);

            return MapToDto(created);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success)
                throw new NotFoundException($"Student with Id {id} was not found.");

            _logger.LogInformation("Deleted student {StudentId}", id);
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _repository.GetAllAsync();
            return students.Select(MapToDto);

        }

        public async Task<StudentDto> GetStudentByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Student with Id {id} was not found.");

            return MapToDto(student);
        }

        public async Task<StudentDto> UpdateStudentAsync(int id, UpdateStudentDto dto)
        {
            var student = await _repository.GetByIdAsync(id)
               ?? throw new NotFoundException($"Student with Id {id} was not found.");

            student.Name = dto.Name;
            student.Email = dto.Email;
            student.Age = dto.Age;
            student.Course = dto.Course;

            await _repository.UpdateAsync(student);
            _logger.LogInformation("Updated student {StudentId}", id);

            return MapToDto(student);
        }

        private static StudentDto MapToDto(Student s) => new()
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            Age = s.Age,
            Course = s.Course,
            CreatedDate = s.CreatedDate
        };
    }
}
