using Auth_Service.Entity;

namespace Auth_Service.Interfaces
{
    public interface IAuthRepository
    {
        Task<Users?> GetUserByUsernameAsync(string username);
        Task<Users?> GetUserByEmailAsync(string email);
        Task<Users> GetUserByIdAsync(int id);
        Task AddUserAsync(Users user);
        Task<Roles> GetRoleByIdAsync(int roleId);
        Task<Roles?> GetRoleByNameAsync(string roleName);
    }
}
