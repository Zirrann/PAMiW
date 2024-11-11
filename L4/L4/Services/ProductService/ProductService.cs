using L4.Models;
using SQLite;
using System.Data;

namespace L4.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly SQLiteAsyncConnection _database;

        public ProductService()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "product.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Product>().Wait();
        }



        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var products = await _database.Table<Product>().ToListAsync();
            return new ServiceResponse<List<Product>> { Data = products, Success = true };
        }

        public async Task<ServiceResponse<Product>> CreateProductAsync(Product newProduct)
        {
            await _database.InsertAsync(newProduct);
            return new ServiceResponse<Product> { Data = newProduct, Success = true };
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int id)
        {
            var product = await _database.Table<Product>().Where(p => p.Id == id).FirstOrDefaultAsync();
            var products = await _database.Table<Product>().ToListAsync();
            if (product == null)
            {
                return new ServiceResponse<bool> { Success = false, Message = "Product not found" };
            }

            var rowsAffected = await _database.DeleteAsync(product);
            return new ServiceResponse<bool> { Data = rowsAffected > 0, Success = true };
        }

        public async Task<ServiceResponse<Product>> UpdateProductAsync(Product updatedProduct)
        {
            await _database.UpdateAsync(updatedProduct);
            return new ServiceResponse<Product> { Data = updatedProduct, Success = true };
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            var product = await _database.FindAsync<Product>(id);
            return new ServiceResponse<Product> { Data = product, Success = product != null };
        }

        public async Task<ServiceResponse<List<Product>>> SearchProductsAsync(string text, int page, int pageSize)
        {
            var products = await _database.Table<Product>()
                                           .Where(p => p.Title.Contains(text))
                                           .Skip(page * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            return new ServiceResponse<List<Product>> { Data = products, Success = true };
        }
    }
}
