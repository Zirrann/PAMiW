using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class OrderService : CrudService<Order, int>, IOrderService
    {
        IProductService _productService;
        public OrderService(AppDbContext dbContext, IProductService productService) : base(dbContext) 
        {
            _productService = productService;
        }



        public override async Task<ServiceReponse<Order>> GetByIdAsync(int id)
        {
            var response = new ServiceReponse<Order>();
            try
            {
                // Pobranie zamówienia z powiązanymi produktami i ich danymi
                var order = await _dbContext.Orders
                    .AsNoTracking()
                    .Include(o => o.OrderProducts)          // Wczytanie powiązanych OrderProducts
                    .ThenInclude(op => op.Product)          // Wczytanie powiązanego produktu
                        .ThenInclude(p => p.Stock)         // Wczytanie powiązanego Stock
                    .Include(o => o.OrderProducts)          // Wczytanie OrderProducts
                        .ThenInclude(op => op.Product)     // Ponowne wczytanie powiązanego produktu
                            .ThenInclude(p => p.Category) // Wczytanie powiązanej kategorii produktu
                    .FirstOrDefaultAsync(o => o.OrderId == id); // Filtrujemy po ID zamówienia

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
                return HandleException<Order>(ex);
            }

            return response;
        }

        public override async Task<ServiceReponse<Order>> CreateAsync(Order order)
        {
            var response = new ServiceReponse<Order>();
            try
            {

                if (order.OrderProducts == null || !order.OrderProducts.Any())
                {
                    throw new Exception("Order must contain at least one product.");
                }


                var validOrderProducts = new List<OrderProduct>();

                foreach (var orderProduct in order.OrderProducts)
                {

                    var productResponse = await _productService.GetByIdAsync(orderProduct.ProductId); 
                    if (!productResponse.Success || productResponse.Data == null)
                    {
                        throw new Exception($"Product with ID {orderProduct.ProductId} not found.");
                    }


                    var existingOrderProduct = await _dbContext.OrderProducts
                        .AsNoTracking()
                        .FirstOrDefaultAsync(op => op.OrderId == order.OrderId && op.ProductId == orderProduct.ProductId);

                    if (existingOrderProduct == null)
                    {
                        validOrderProducts.Add(new OrderProduct
                        {
                            OrderId = order.OrderId,
                            ProductId = orderProduct.ProductId,
                            Order = order,
                            Product = productResponse.Data 
                        });
                    }
                    else
                    {
                        validOrderProducts.Add(existingOrderProduct);
                    }
                }

                order.OrderProducts = validOrderProducts;

                await _dbContext.Orders.AddAsync(order);

                await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Data = order;
                response.Message = "Order created successfully.";
            }
            catch (Exception ex)
            {
                return HandleException<Order>(ex);
            }



            return response;
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
                return HandleException<IEnumerable<Order>>(ex);
            }
            return response;
        }

    }
}
