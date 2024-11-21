using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class OrderProductService : CrudService<OrderProduct, OrderProductKey>, IOrderProductService
    {
        public OrderProductService(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
