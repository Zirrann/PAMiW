using Shared.Models.Dto;

namespace Shop.MVC.Models
{
    public class OrderDetailsModel
    {
        public OrderDto Order { get; set; }
        public IEnumerable<ProductDto> AvailableProducts { get; set; }
        public IEnumerable<ProductDto> SelectedProducts { get; set; }
    }

}
