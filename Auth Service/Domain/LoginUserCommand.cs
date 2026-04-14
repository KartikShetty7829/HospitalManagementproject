using MediatR;

namespace Auth_Service.Domain
{
   
        public class LoginUserCommand : IRequest<LoginUserResponseDto>
        {
            public required string Username { get; set; } 
            public required string Password { get; set; } 
        }
    
}
