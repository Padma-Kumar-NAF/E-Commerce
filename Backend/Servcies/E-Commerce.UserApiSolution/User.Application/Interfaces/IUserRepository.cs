using System;
using System.Collections.Generic;
using System.Text;
using User.Domain.Entities;

namespace User.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByEmail(string email);
    }
}
