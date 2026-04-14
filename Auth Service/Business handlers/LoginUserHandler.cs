using Auth_Service.Domain;
using Auth_Service.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth_Service.Business_handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResponseDto>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<LoginUserHandler> _logger;
        private readonly IJwtService _jwtService;

        public LoginUserHandler(IAuthRepository authRepository, ILogger<LoginUserHandler> logger, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _logger = logger;
            _jwtService = jwtService;
        }

        public async Task<LoginUserResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("LoginUserHandler started for Username: {Username}", request.Username);

                // Find user
                var user = await _authRepository.GetUserByUsernameAsync(request.Username);
                if (user == null)
                {
                    _logger.LogInformation("Login failed: Username {Username} not found", request.Username);
                    return new LoginUserResponseDto
                    {
                        Token = string.Empty,
                        RoleName = string.Empty,
                        Message = "Invalid username or password."
                    };
                }

                // ✅ Just check plain password equality (no hashing)
                if (request.Password != user.PasswordHash)
                {
                    _logger.LogInformation("Login failed: Invalid password for Username {Username}", request.Username);
                    return new LoginUserResponseDto
                    {
                        Token = string.Empty,
                        RoleName = string.Empty,
                        Message = "Invalid username or password."
                    };
                }


                // Get role
                var role = await _authRepository.GetRoleByIdAsync(user.RoleId);
                if (role == null)
                {
                    _logger.LogWarning("Login failed: Role not found for Username {Username}", request.Username);
                    return new LoginUserResponseDto
                    {
                        Token = string.Empty,
                        RoleName = string.Empty,
                        Message = "User role not found."
                    };
                }

                // Generate JWT
                var token = _jwtService.GenerateJwtToken(user, role.RoleName);

                _logger.LogInformation("User {Username} logged in successfully with Role {RoleName}", user.Username, role.RoleName);

                return new LoginUserResponseDto
                {
                    Token = token,
                    RoleName = role.RoleName,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error occurred while logging in user {Username}", request.Username);

                return new LoginUserResponseDto
                {
                    Token = string.Empty,
                    RoleName = string.Empty,
                    Message = "Login failed due to an internal error."
                };
            }
        }
    }

}
