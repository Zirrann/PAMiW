using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class OrderProductService : CrudService<OrderProduct, OrderProductKey>, IOrderProductService
    {
        IProductService _productService;
        IOrderService _orderService;

        public OrderProductService(AppDbContext dbContext, IProductService productService, IOrderService orderService) : base(dbContext)
        {
            _productService = productService;
            _orderService = orderService;
        }


        public override async Task<ServiceReponse<IEnumerable<OrderProduct>>> GetAllAsync()
        {
            var response = new ServiceReponse<IEnumerable<OrderProduct>>();
            try
            {
                // Pobranie wszystkich OrderProduct z powiązanymi danymi
                var orderProducts = await _dbContext.OrderProducts
                    .AsNoTracking()
                    .Include(op => op.Order)
                    .Include(op => op.Product)
                        .ThenInclude(p => p.Stock)
                    .Include(op => op.Product)
                        .ThenInclude(p => p.Category)
                    .ToListAsync();

                response.Data = orderProducts;
                response.Success = true;
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<OrderProduct>>(ex);
            }
            return response;
        }



        public override async Task<ServiceReponse<OrderProduct>> GetByIdAsync(OrderProductKey id)
        {
            var response = new ServiceReponse<OrderProduct>();
            try
            {
                // Pobranie OrderProduct z powiązanymi danymi
                var orderProduct = await _dbContext.OrderProducts
                    .AsNoTracking()
                    .Include(op => op.Order)              // Załadowanie danych związanych z zamówieniem
                    .Include(op => op.Product)           // Załadowanie danych związanych z produktem
                        .ThenInclude(p => p.Stock)       // Załadowanie danych związanych ze stanem magazynowym
                    .Include(op => op.Product)           // Załadowanie danych związanych z kategorią produktu
                        .ThenInclude(p => p.Category)
                    .FirstOrDefaultAsync(op => op.OrderId == id.OrderId && op.ProductId == id.ProductId);

                if (orderProduct == null)
                {
                    response.Success = false;
                    response.Message = "OrderProduct not found.";
                }
                else
                {
                    response.Data = orderProduct;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                return HandleException<OrderProduct>(ex);
            }
            return response;
        }




        public override async Task<ServiceReponse<OrderProduct>> CreateAsync(OrderProduct orderProduct)
        {
            var response = new ServiceReponse<OrderProduct>();
            try
            {
                if (orderProduct.OrderId == 0 || orderProduct.ProductId == 0)
                {
                    throw new Exception("OrderId and ProductId must be provided.");
                }

                var orderResponse = await _orderService.GetByIdAsync(orderProduct.OrderId);
                var productResponse = await _productService.GetByIdAsync(orderProduct.ProductId);

                if (!orderResponse.Success || orderResponse.Data == null)
                {
                    throw new Exception($"Order with ID {orderProduct.OrderId} not found.");
                }

                if (!productResponse.Success || productResponse.Data == null)
                {
                    throw new Exception($"Product with ID {orderProduct.ProductId} not found.");
                }


                var existingOrder = orderResponse.Data;
                var existingProduct = productResponse.Data;

                _dbContext.Entry(existingOrder).State = EntityState.Unchanged;
                _dbContext.Entry(existingProduct).State = EntityState.Unchanged;

                orderProduct.Order = existingOrder;
                orderProduct.Product = existingProduct;

                await _dbContext.OrderProducts.AddAsync(orderProduct);
                await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Data = orderProduct;
            }
            catch (Exception ex)
            {
                return HandleException<OrderProduct>(ex);
            }

            return response;
        }


        public override async Task<ServiceReponse<bool>> DeleteAsync(OrderProductKey id)
        {
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(op => op.OrderId == id.OrderId && op.ProductId == id.ProductId);
                if (entity == null)
                {
                    return new ServiceReponse<bool>
                    {
                        Success = false,
                        Message = "Entity not found.",
                        Data = false
                    };
                }

                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return new ServiceReponse<bool>
                {
                    Data = true,
                    Success = true,
                    Message = "Entity deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex);
            }
        }
    }
}
