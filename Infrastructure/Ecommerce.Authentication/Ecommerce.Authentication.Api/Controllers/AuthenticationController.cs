using Ecommerce.Authentication.Application.Interfaces;
using Ecommerce.Authentication.Domain.Entities;
using Ecommerce.Authentication.Domain.Requests;
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
        public async Task<ActionResult<Customer>> Register([FromBody] RegisterRequest request)
        {
            var customer = await _authenticationService.RegisterAsync(request);
            return Ok(customer);
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
