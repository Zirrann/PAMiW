using Shared.Models;
using Shared.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.DB.Services
{
    public interface IOrderService : ICrudService<Order, int>
    {
        Task<ServiceReponse<IEnumerable<Order>>> GetOrdersWithOrderProductsAsync();
        Task<ServiceReponse<Order>> GetOrderWithOrderProductsByIdAsync(int orderId);
    }
}
