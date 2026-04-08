using Auth_Service.Domain;
using Auth_Service.Entity;
using Auth_Service.Interfaces;
using MediatR;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Auth_Service.Business_handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponseDto>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger _logger;

        public RegisterUserHandler(IAuthRepository authRepository, ILogger logger)
        {
            _authRepository = authRepository;
            _logger = logger;
        }

        public async Task<RegisterUserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("RegisterUserHandler started for Username: {Username}, Email: {Email}",
                    request.Username, request.Email);

                // Check duplicates
                var existingByUsername = await _authRepository.GetUserByUsernameAsync(request.Username);
                if (existingByUsername != null)
                {
                    _logger.Warning("Duplicate registration attempt for Username: {Username}", request.Username);
                    return new RegisterUserResponseDto
                    {
                        RoleName = string.Empty,
                        Message = "Registration failed: Username already exists."
                    };
                }

                var existingByEmail = await _authRepository.GetUserByEmailAsync(request.Email);
                if (existingByEmail != null)
                {
                    _logger.Warning("Duplicate registration attempt for Email: {Email}", request.Email);
                    return new RegisterUserResponseDto
                    {
                        RoleName = string.Empty,
                        Message = "Registration failed: Email already exists."
                    };
                }

                // Get role
                var role = await _authRepository.GetRoleByNameAsync(request.RoleName);
                if (role == null)
                {
                    _logger.Warning("Role {RoleName} not found", request.RoleName);
                    return new RegisterUserResponseDto
                    {
                        RoleName = string.Empty,
                        Message = "Registration failed: Role not found."
                    };
                }

                // Create new user
                var user = new Users
                {
                    Username = request.Username,
                    PasswordHash = request.Password, // TODO: hash properly
                    Email = request.Email,
                    RoleId = role.RoleId,   // ✅ set FK
                    Role = role             // ✅ set navigation property
                };

                await _authRepository.AddUserAsync(user);

                _logger.Information("User {Username} registered successfully with Role {RoleName}",
                    user.Username, role.RoleName);

                return new RegisterUserResponseDto
                {
                    RoleName = role.RoleName,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while registering user {Username}", request.Username);

                return new RegisterUserResponseDto
                {
                    RoleName = string.Empty,
                    Message = "Registration failed due to an internal error."
                };
            }
        }
    }
}
