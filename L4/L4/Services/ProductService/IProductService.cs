using L4.Models;


namespace L4.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> CreateProductAsync(Product newProduct);
        Task<ServiceResponse<bool>> DeleteProductAsync(int id);
        Task<ServiceResponse<Product>> UpdateProductAsync(Product updatedProduct);
        Task<ServiceResponse<Product>> GetProductAsync(int id);

        Task<ServiceResponse<List<Product>>> SearchProductsAsync(string text, int page, int pageSize);

    }
}
