using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Orders.Domain.Dtos
{
    public class OrderDto
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
