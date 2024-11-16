namespace Shop.DB.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockId { get; set; }
        public StockDto Stock { get; set; } // Zawiera tylko niezbędne dane stanu magazynowego
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
