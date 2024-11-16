using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class OrderService : CrudService<Order, int>, IOrderService
    {
        public OrderService(AppDbContext dbContext) : base(dbContext) { }

        // Custom method to get orders with their related OrderProducts
        public async Task<ServiceReponse<IEnumerable<Order>>> GetOrdersWithOrderProductsAsync()
        {
            try
            {
                var orders = await _dbContext.Orders
                    .AsNoTracking()
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .ToListAsync();

                return new ServiceReponse<IEnumerable<Order>> { Data = orders, Success = true };
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Order>>(ex);
            }
        }

        public override async Task<ServiceReponse<IEnumerable<Order>>> GetAllAsync()
        {
            var response = new ServiceReponse<IEnumerable<Order>>();
            try
            {
                var orders = await _dbContext.Orders
                    .AsNoTracking()
                    .Include(o => o.OrderProducts)         // Wczytanie powiązań z OrderProducts
                        .ThenInclude(op => op.Product)    // Wczytanie powiązanych produktów
                            .ThenInclude(p => p.Stock)    // Wczytanie powiązanego Stock
                    .Include(o => o.OrderProducts)        // Ponowne wczytanie OrderProducts
                        .ThenInclude(op => op.Product)    // Wczytanie powiązanych produktów
                            .ThenInclude(p => p.Category) // Wczytanie powiązanej kategorii
                    .ToListAsync();

                response.Data = orders;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }





        // Custom method to get order by its Id with OrderProducts
        public async Task<ServiceReponse<Order>> GetOrderWithOrderProductsByIdAsync(int orderId)
        {
            var response = new ServiceReponse<Order>();
            try
            {
                var order = await _dbContext.Orders
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    response.Success = false;
                    response.Message = "Order not found.";
                }
                else
                {
                    response.Data = order;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
