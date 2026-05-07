using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using User.Application.Interfaces;
using User.Domain.Entities;
using User.Infrastructure.Data;

namespace User.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
