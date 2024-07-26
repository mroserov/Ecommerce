using Ecommerce.Authentication.Api.Requests;
using Ecommerce.Authentication.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Authentication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authenticationService.LoginAsync(request.Email, request.Password);
            if (token == null)
            {
                return NotFound();
            }
            if (token == string.Empty)
            {
                return Unauthorized();
            }

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authenticationService.RegisterAsync(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Address, request.Password);
            return Ok();
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var currentToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var token = await _authenticationService.RefreshTokenAsync(currentToken);
            if (string.IsNullOrEmpty(currentToken))
            {
                return Unauthorized();
            }

            return Ok(new { Token = currentToken });
        }
    }
}
