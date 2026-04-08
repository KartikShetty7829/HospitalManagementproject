
using MediatR;
namespace Auth_Service.Domain
{
    public class RegisterUserResponseDto
    {
        public string RoleName { get; set; }
        public string Message { get; set; }
    }
}
