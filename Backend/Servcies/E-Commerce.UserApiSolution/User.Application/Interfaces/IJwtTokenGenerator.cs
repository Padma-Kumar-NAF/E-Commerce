using User.Domain.Entities;

namespace User.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserEntity user);
    }
}