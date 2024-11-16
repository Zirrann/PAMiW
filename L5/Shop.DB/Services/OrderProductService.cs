using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class OrderProductService : CrudService<OrderProduct, int>, IOrderProductService
    {
        public OrderProductService(AppDbContext dbContext) : base(dbContext) { }


        public override async Task<ServiceReponse<IEnumerable<OrderProduct>>> GetAllAsync()
        {
            var response = new ServiceReponse<IEnumerable<OrderProduct>>();
            try
            {
                // Pobranie wszystkich OrderProduct z powiązanymi danymi
                var orderProducts = await _dbContext.OrderProducts
                    .AsNoTracking()
                    .Include(op => op.Order)              // Wczytanie powiązanego Order
                    .Include(op => op.Product)           // Wczytanie powiązanego Product
                        .ThenInclude(p => p.Stock)       // Wczytanie powiązanego Stock
                    .Include(op => op.Product)           // Ponowne wczytanie Product
                        .ThenInclude(p => p.Category)    // Wczytanie powiązanej kategorii
                    .ToListAsync();

                response.Data = orderProducts;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }


        public override async Task<ServiceReponse<OrderProduct>> CreateAsync(OrderProduct orderProduct)
        {
            var response = new ServiceReponse<OrderProduct>();
            try
            {
                // Ensure OrderId and ProductId are provided
                if (orderProduct.OrderId == 0 || orderProduct.ProductId == 0)
                {
                    throw new Exception("OrderId and ProductId must be provided.");
                }

                // Check if the Order and Product exist
                var order = await _dbContext.Orders.FindAsync(orderProduct.OrderId);
                var product = await _dbContext.Products.FindAsync(orderProduct.ProductId);

                if (order == null || product == null)
                {
                    throw new Exception("Order or Product not found.");
                }

                // Now create the OrderProduct and save to context
                await _dbContext.OrderProducts.AddAsync(orderProduct);

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Data = orderProduct;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }





        // Custom method: Get OrderProducts by OrderId
        public async Task<ServiceReponse<IEnumerable<OrderProduct>>> GetOrderProductsByOrderIdAsync(int orderId)
        {
            var response = new ServiceReponse<IEnumerable<OrderProduct>>();
            try
            {
                response.Data = await _dbSet
                    .Where(op => op.OrderId == orderId)
                    .Include(op => op.Product) // Including related Product entity
                    .ToListAsync();

                response.Success = true;
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
