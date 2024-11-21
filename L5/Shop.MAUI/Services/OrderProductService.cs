using Shared.Models;
using Shared.Services;
using Shared.Models.Dto;

namespace Shop.MAUI.Services
{
    public class OrderProductService : CrudService<OrderProduct, OrderProductKey>, IOrderProductService
    {
        public OrderProductService(HttpClient httpClient)
            : base(httpClient, "api/orderproducts") // Ścieżka API
        {
        }
    }

}
