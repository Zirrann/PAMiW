using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Models.Dto;
using Shared.Services;
using System.Collections.Generic;

namespace Shop.DB.Services
{
    public class OrderProductService : CrudService<OrderProduct, OrderProductKey>, IOrderProductService
    {
        public OrderProductService(AppDbContext dbContext) : base(dbContext)
        {
        }

        override public async Task<ServiceReponse<OrderProduct>> GetByIdAsync(OrderProductKey key)
        {
            var entity = await _dbContext.OrderProducts
                .FirstOrDefaultAsync(op => op.OrderId == key.OrderId && op.ProductId == key.ProductId);

            if (entity == null)
            {
                return new ServiceReponse<OrderProduct>
                {
                    Success = false,
                    Message = "OrderProduct not found"
                };
            }

            return new ServiceReponse<OrderProduct>
            {
                Success = true,
                Data = new OrderProduct
                {
                    OrderId = entity.OrderId,
                    ProductId = entity.ProductId
                },
                Message = "OrderProduct retrieved successfully"
            };
        }

        public async Task<ServiceReponse<bool>> DeleteAsync(OrderProductKey key)
        {
            var entity = await _dbContext.OrderProducts
                .FirstOrDefaultAsync(op => op.OrderId == key.OrderId && op.ProductId == key.ProductId);


            if (entity == null)
            {
                return new ServiceReponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "OrderProduct not found"
                };
            }

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return new ServiceReponse<bool>
            {
                Success = true,
                Data = true,
                Message = "OrderProduct deleted successfully"
            };
        }
    }

}
