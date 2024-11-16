using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IProductService : ICrudService<Product, int>
    {
        Task<ServiceReponse<IEnumerable<Product>>> GetProductsByCategoryIdAsync(int categoryId);
        Task<ServiceReponse<Product>> GetProductWithStockAsync(int productId);
    }
}
