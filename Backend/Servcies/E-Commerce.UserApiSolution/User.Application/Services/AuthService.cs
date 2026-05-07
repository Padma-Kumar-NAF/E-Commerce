using User.Application.DTOs;
using User.Application.Interfaces;

namespace Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;

    public AuthService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Login(LoginRequestDto dto)
    {
        var user = await _repository.GetByEmail(dto.Email);

        if (user == null)
        {
            return false;
        }

        if (user.Password != dto.Password)
        {
            return false;
        }

        return true;
    }
}