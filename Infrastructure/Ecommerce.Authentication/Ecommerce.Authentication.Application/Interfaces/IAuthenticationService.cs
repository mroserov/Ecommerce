using Ecommerce.Authentication.Domain.Entities;
using Ecommerce.Authentication.Domain.Requests;

namespace Ecommerce.Authentication.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string?> LoginAsync(string email, string password);
        Task<string?> RefreshTokenAsync(string token);
        Task<CustomerResponse> RegisterAsync(RegisterRequest customer);
    }
}
