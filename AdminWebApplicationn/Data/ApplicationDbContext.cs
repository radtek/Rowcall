using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MomensoBackend.Models;
using RowcallBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MomensoBackend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ClassRoom> ClassRoom { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<UserClass> UserClass { get; set; }
        public DbSet<UserToken> UserToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserClass>().HasKey(k => new { k.ApplicationUserId, k.ClassRoomId });
            modelBuilder.Entity<UserToken>().HasKey(k => new { k.ApplicationUserId, k.TokenId });

        }
    }
}
