namespace Shop.DB.DTO
{
    public class StockDto
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
        // Pominięto produkt, aby uniknąć cyklu
    }
}
