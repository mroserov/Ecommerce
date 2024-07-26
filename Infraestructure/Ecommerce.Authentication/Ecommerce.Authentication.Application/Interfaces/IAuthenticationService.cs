namespace Ecommerce.Authentication.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string?> LoginAsync(string email, string password);
        Task<string?> RefreshTokenAsync(string token);
        Task RegisterAsync(string firstName, string lastName, string email, string phoneNumber, string address, string password);
    }
}
