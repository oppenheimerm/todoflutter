using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoFlutter.core.Models;

namespace TodoFlutter.data
{
    public class AppDbContext : IdentityDbContext<AppUser, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Todo> ToDos { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
