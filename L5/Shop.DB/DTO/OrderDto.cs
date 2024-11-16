﻿namespace Shop.DB.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public ICollection<OrderProductDto> OrderProducts { get; set; }
    }
}
