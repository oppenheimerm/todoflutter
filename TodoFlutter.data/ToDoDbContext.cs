using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models;

namespace TodoFlutter.data
{
    public class ToDoDbContext : IdentityDbContext<AppUser, Role, string>
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options):base(options)
        {

        }

        public DbSet<Todo> ToDos { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
