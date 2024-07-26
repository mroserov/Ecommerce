using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Authentication.Api.Requests
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[A-Z]).+$", ErrorMessage = "Password must contain at least one uppercase letter")]
        public string Password { get; set; }
    }
}
