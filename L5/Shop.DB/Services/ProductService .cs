using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class ProductService : CrudService<Product, int>, IProductService
    {
        public ProductService(AppDbContext dbContext) : base(dbContext) { }

        public override async Task<ServiceReponse<IEnumerable<Product>>> GetAllAsync()
        {
            var response = new ServiceReponse<IEnumerable<Product>>();
            try
            {
                var products = await _dbContext.Products
                    .Include(p => p.Stock)      // Załaduj powiązany Stock
                    .Include(p => p.Category)   // Załaduj powiązaną kategorię
                    .ToListAsync();

                response.Data = products;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public override async Task<ServiceReponse<Product>> CreateAsync(Product product)
        {
            var response = new ServiceReponse<Product>();
            try
            {
                // Walidacja kategorii
                if (product.CategoryId > 0 &&
                    !await _dbContext.Categories.AnyAsync(c => c.CategoryId == product.CategoryId))
                {
                    response.Success = false;
                    response.Message = "Invalid CategoryId.";
                    return response;
                }

                // Tworzenie Stock, jeśli istnieje
                if (product.Stock != null)
                {
                    _dbContext.Stocks.Add(product.Stock);
                }

                _dbSet.Add(product);
                await _dbContext.SaveChangesAsync();

                response.Data = product;
                response.Success = true;
                response.Message = "Product created successfully.";
            }
            catch (Exception ex)
            {
                return HandleException<Product>(ex);
            }
            return response;
        }



        public async Task<ServiceReponse<IEnumerable<Product>>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var response = new ServiceReponse<IEnumerable<Product>>();
            try
            {
                var products = await _dbSet
                    .Where(p => p.CategoryId == categoryId)
                    .Include(p => p.Stock) // Załaduj relację 1:1
                    .ToListAsync();

                response.Data = products;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceReponse<Product>> GetProductWithStockAsync(int productId)
        {
            var response = new ServiceReponse<Product>();
            try
            {
                var product = await _dbSet
                    .Include(p => p.Stock) // Relacja 1:1
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Product not found.";
                }
                else
                {
                    response.Data = product;
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