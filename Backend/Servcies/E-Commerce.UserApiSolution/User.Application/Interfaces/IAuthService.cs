using User.Application.DTOs;

namespace User.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> Login(LoginRequestDto request);
    }
}