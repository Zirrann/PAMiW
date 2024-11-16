using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IOrderProductService : ICrudService<OrderProduct, int>
    {
        Task<ServiceReponse<IEnumerable<OrderProduct>>> GetOrderProductsByOrderIdAsync(int orderId);
    }
}
