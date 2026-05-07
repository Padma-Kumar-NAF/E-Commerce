using System;
using System.Collections.Generic;
using System.Text;
using User.Application.DTOs;

namespace User.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(LoginRequestDto dto);
    }
}
