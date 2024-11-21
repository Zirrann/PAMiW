namespace Shared.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockId { get; set; }
        public int CategoryId { get; set; }
    }

}
