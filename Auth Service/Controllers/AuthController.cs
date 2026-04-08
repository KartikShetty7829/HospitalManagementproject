using Auth_Service.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth_Service.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _mediator.Send(command);

            // Map messages to status codes
            if (response.Message.Contains("Username already exists") ||
                response.Message.Contains("Email already exists"))
            {
                return Conflict(response); // 409 Conflict
            }

            if (response.Message.Contains("Role not found"))
            {
                return NotFound(response); // 404 Not Found
            }

            if (response.Message.Contains("internal error"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response); // 200 Success
        }

    }
}
