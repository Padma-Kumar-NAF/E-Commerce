using User.Application.DTOs;
using User.Application.Interfaces;

namespace Auth.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository repository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _repository = repository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto dto)
        {
            var user = await _repository.GetByEmail(dto.Email);

            if (user == null)
                return null;

            if (user.Password != dto.Password)
                return null;

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };
        }
    }
}