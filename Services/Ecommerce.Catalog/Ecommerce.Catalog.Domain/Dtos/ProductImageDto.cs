using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Catalog.Domain.Dtos;

public class ProductImageDto
{
    [Required]
    public IFormFile Image { get; set; }
}
