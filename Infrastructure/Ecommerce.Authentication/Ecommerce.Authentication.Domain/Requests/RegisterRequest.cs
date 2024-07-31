using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Authentication.Domain.Requests;

public class RegisterRequest : LoginRequest
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [StringLength(200)]
    public string Address { get; set; }
}
