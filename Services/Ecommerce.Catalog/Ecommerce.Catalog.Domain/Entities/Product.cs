using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Catalog.Domain.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [DefaultValue(0)]
        public decimal Discount { get; set; } = 0;

        public string? Slug { get; set; }

        public List<Category> Categories { get; set; }

        public string? ImageUrl { get; set; }
    }
}
