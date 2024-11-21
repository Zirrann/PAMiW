namespace Shared.Models.Dto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        // Pominięto produkty, aby uniknąć cyklu
    }
}
