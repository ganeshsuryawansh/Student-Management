using StudentManagementSystem.Models.DTOs;

namespace StudentManagementSystem.Services
{
    public interface ITokenService
    {
        LoginResponseDto GenerateToken(string username);
    }
}
