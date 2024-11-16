namespace Shop.DB.DTO
{
    public class OrderProductDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; } // Zawiera tylko niezbędne dane produktu
    }
}
