    using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class ProductService : CrudService<Product, int>, IProductService
    {
        ICategoryService _categoryService;
        IStockService _stockService;

        public ProductService(AppDbContext dbContext, ICategoryService categoryService, IStockService stockService) : base(dbContext) 
        { 
            _categoryService = categoryService;
            _stockService = stockService;
        }

        public override async Task<ServiceReponse<IEnumerable<Product>>> GetAllAsync()
        {
            var response = new ServiceReponse<IEnumerable<Product>>();
            try
            {
                var products = await _dbContext.Products
                    .Include(p => p.Stock)      
                    .Include(p => p.Category)  
                    .ToListAsync();

                response.Data = products;
                response.Success = true;
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex);
            }
            return response;
        }


        public override async Task<ServiceReponse<Product>> GetByIdAsync(int id)
        {
            var response = new ServiceReponse<Product>();
            try
            {
                var product = await _dbContext.Products
                    .Include(p => p.Stock)     
                    .Include(p => p.Category)   
                    .FirstOrDefaultAsync(p => p.Id.Equals(id)); 

                // Jeśli produkt nie został znaleziony
                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Product not found.";
                    return response;
                }

                // Zwróć produkt w odpowiedzi
                response.Data = product;
                response.Success = true;
            }
            catch (Exception ex)
            {
                return HandleException<Product>(ex);
            }
            return response;
        }


        public override async Task<ServiceReponse<Product>> CreateAsync(Product product)
        {
            var response = new ServiceReponse<Product>();
            try
            {
                // Walidacja i pobranie kategorii
                if (product.CategoryId > 0)
                {
                    var categoryResponse = await _categoryService.GetByIdAsync(product.CategoryId);
                    if (!categoryResponse.Success)
                    {
                        response.Success = false;
                        response.Message = "Invalid CategoryId.";
                        return response;
                    }
                    product.Category = categoryResponse.Data;
                }

                // Walidacja i pobranie stocku
                if (product.Stock != null)
                {
                    var stockResponse = await _stockService.GetByIdAsync(product.StockId);
                    if (!stockResponse.Success)
                    {
                        // Jeśli StockId nie istnieje, utwórz nowy stock
                        var newStockResponse = await _stockService.CreateAsync(product.Stock);
                        if (!newStockResponse.Success)
                        {
                            response.Success = false;
                            response.Message = "Failed to create Stock.";
                            return response;
                        }
                        product.Stock = newStockResponse.Data;
                    }
                    else
                    {
                        product.Stock = stockResponse.Data;
                    }
                }

                // Dodanie produktu
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

    }
}