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


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Roles table mapping
        //    modelBuilder.Entity<Role>(entity =>
        //    {
        //        entity.ToTable("Roles");

        //        entity.HasKey(r => r.RoleId);

        //        entity.Property(r => r.RoleId).HasColumnName("RoleId");
        //        entity.Property(r => r.RoleName)
        //              .HasColumnName("RoleName")
        //              .HasMaxLength(50)
        //              .IsRequired();
        //    });

        //    // Users table mapping
        //    modelBuilder.Entity<User>(entity =>
        //    {
        //        entity.ToTable("Users");

        //        entity.HasKey(u => u.UserId);

        //        entity.Property(u => u.UserId).HasColumnName("UserId");
        //        entity.Property(u => u.Username)
        //              .HasColumnName("Username")
        //              .HasMaxLength(50)
        //              .IsRequired();
        //        entity.Property(u => u.PasswordHash)
        //              .HasColumnName("PasswordHash")
        //              .HasMaxLength(255)
        //              .IsRequired();
        //        entity.Property(u => u.Email)
        //              .HasColumnName("Email")
        //              .HasMaxLength(100);
        //        entity.Property(u => u.RoleId)
        //              .HasColumnName("RoleId")
        //              .IsRequired();
        //        entity.Property(u => u.CreatedAt)
        //              .HasColumnName("CreatedAt")
        //              .HasDefaultValueSql("GETDATE()");

        //        entity.HasOne(u => u.Role)
        //              .WithMany()
        //              .HasForeignKey(u => u.RoleId);
        //    });
    }
}


