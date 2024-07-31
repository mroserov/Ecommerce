using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Authentication.Domain.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string Token { get; set; }
}
