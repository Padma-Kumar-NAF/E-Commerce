using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using User.Domain.Entities;

namespace User.Infrastructure.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
        {
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
    }
}
