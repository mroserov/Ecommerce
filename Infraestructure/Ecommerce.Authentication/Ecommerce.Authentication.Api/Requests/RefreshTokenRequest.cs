using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Authentication.Api.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
