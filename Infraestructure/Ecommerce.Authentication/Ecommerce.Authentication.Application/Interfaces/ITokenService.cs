using Ecommerce.Authentication.Domain.Entities;
using System.Security.Claims;

namespace Ecommerce.Authentication.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(Customer customer);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
