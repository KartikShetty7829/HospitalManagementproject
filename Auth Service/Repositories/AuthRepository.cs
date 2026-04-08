using Auth_Service.Data;
using Auth_Service.Entity;
using Auth_Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Auth_Service.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthDbContext _context;

        public AuthRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Roles?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

       

        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }


        public async Task AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Roles?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

       
    }
}
