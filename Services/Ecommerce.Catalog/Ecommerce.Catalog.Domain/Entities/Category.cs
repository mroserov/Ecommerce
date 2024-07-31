using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Catalog.Domain.Entities;

public class Category : BaseEntity
{
    [Required]
    public string Name { get; set; }

    public List<Product> Products { get; set; }
}
