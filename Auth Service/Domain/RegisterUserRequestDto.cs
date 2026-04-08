using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Auth_Service.Domain
{
    public class RegisterUserCommand : IRequest<RegisterUserResponseDto>
    {
        //
        [Required, MinLength(3)]
        public required string Username { get; set; }

        [Required, MinLength(6)]
        public required string Password { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        [RegularExpression("Receptionist|Doctor|Admin", ErrorMessage = "Role must be Receptionist, Doctor, or Admin")]
        public required string RoleName { get; set; }
    }
}
