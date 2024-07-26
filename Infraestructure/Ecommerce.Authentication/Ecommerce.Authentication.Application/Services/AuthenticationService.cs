using Ecommerce.Authentication.Application.Interfaces;
using Ecommerce.Authentication.Domain.Entities;
using Ecommerce.Authentication.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        public async Task RegisterAsync(string firstName, string lastName, string email, string phoneNumber, string address, string password)
        {
            var userExist = await _unitOfWork.Customers.GetByEmailAsync(email);

            if (userExist != null)
            {
                throw new InvalidOperationException("The email is already registered");
            }
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                RefreshToken = _tokenService.GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.Customers.AddAsync(customer);
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
