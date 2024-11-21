    using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class ProductService : CrudService<Product, int>, IProductService
    {
        public ProductService(AppDbContext dbContext) : base(dbContext) 
        { 
        }
    }
}