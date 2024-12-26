
using Shared.Models.Dto;

namespace Shop.MVC.Models
{
    public class ProductDetailsModel
    {
        public ProductDto Product { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }

    }
}
