using System.Collections.Generic;

namespace Shared.Models.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }

        public List<OrderProductDto> OrderProducts { get; set; }
    }
}
