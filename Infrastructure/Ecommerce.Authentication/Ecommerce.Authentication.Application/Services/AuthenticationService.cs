using Ecommerce.Authentication.Application.Interfaces;
using Ecommerce.Authentication.Domain.Entities;
using Ecommerce.Authentication.Domain.Interfaces;
using Ecommerce.Authentication.Domain.Requests;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Ecommerce.Authentication.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var customer = await _unitOfWork.Customers.GetByEmailAsync(email);
            if (customer == null)
            {
                return null;
            }
            else if (!BCrypt.Net.BCrypt.Verify(password, customer.PasswordHash))
            {
                return string.Empty;
            }

            var token = _tokenService.GenerateJwtToken(customer);
            var refreshToken = _tokenService.GenerateRefreshToken();

            customer.RefreshToken = refreshToken;
            customer.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Refresh token válido por 7 días

            await _unitOfWork.Customers.UpdateAsync(customer);

            return token;
        }

        public async Task<CustomerResponse> RegisterAsync(RegisterRequest customerRequest)
        {
            var userExist = await _unitOfWork.Customers.GetByEmailAsync(customerRequest.Email);

            if (userExist != null)
            {
                throw new InvalidOperationException("The email is already registered");
            }
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = customerRequest.FirstName,
                LastName = customerRequest.LastName,
                Email = customerRequest.Email,
                PhoneNumber = customerRequest.PhoneNumber,
                Address = customerRequest.Address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(customerRequest.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                RefreshToken = _tokenService.GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            var customerResponse = await _unitOfWork.Customers.AddAsync(customer) ?? throw new InvalidOperationException("Error creando usuario");

            return new CustomerResponse()
            {
                Address = customerResponse.Address,
                FirstName = customerResponse.FirstName,
                Email = customerResponse.Email,
                Id = customerResponse.Id,
                LastName = customerResponse.LastName,
                PhoneNumber = customerResponse.PhoneNumber
            };
        }

        public async Task<string?> RefreshTokenAsync(string token)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email) ?? throw new SecurityTokenException("Invalid token");
            var email = emailClaim.Value;

            var customer = await _unitOfWork.Customers.GetByEmailAsync(email);
            if (customer == null || customer.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var newJwtToken = _tokenService.GenerateJwtToken(customer);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            customer.RefreshToken = newRefreshToken;
            customer.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Customers.UpdateAsync(customer);

            return newJwtToken;
        }
    }
}
