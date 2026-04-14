namespace Auth_Service.Domain
{
    public class LoginUserResponseDto
    {
        public required string Token { get; set; }    // JWT or session token
        public required string RoleName { get; set; } // User’s role
        public  required string Message { get; set; }  // Success/failure message
    }
}
