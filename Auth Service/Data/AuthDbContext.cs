using Auth_Service.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Auth_Service.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        // DbSets map to tables
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationship between Users and Role
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // Unique index on Username
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Unique index on Email
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}


