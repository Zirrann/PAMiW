using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;
using Shop.DB.DTO; // Zakładam, że masz ProductDto w tym namespace
using Microsoft.EntityFrameworkCore;

namespace Shop.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ProductsController(IProductService productService, IMapper mapper, AppDbContext context)
        {
            _productService = productService;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productService.GetAllAsync();
            if (response.Success)
            {
                // Mapowanie produktów na ProductDto
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(response.Data);
                return Ok(productDtos);
            }
            return StatusCode(500, response.Message);
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await _productService.GetByIdAsync(id);
            if (response.Success)
            {
                // Mapowanie produktu na ProductDto
                var productDto = _mapper.Map<ProductDto>(response.Data);
                return Ok(productDto);
            }
            if (response.Data == null)
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
                return BadRequest("Product cannot be null.");

            var product = _mapper.Map<Product>(productDto);  // Nie ustawiamy Id ręcznie

            var response = await _productService.CreateAsync(product);  // EF Core przypisuje Id automatycznie
            if (response.Success)
            {
                var createdProductDto = _mapper.Map<ProductDto>(response.Data);
                return CreatedAtAction(nameof(GetProductById), new { id = createdProductDto.Id }, createdProductDto);
            }
            return StatusCode(500, response.Message);
        }



        // PUT: api/Products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (productDto == null || productDto.Id != id)
                return BadRequest("Product data is invalid.");

            // Sprawdzenie, czy stock i category istnieją
            var stock = await _context.Stocks.FindAsync(productDto.StockId);
            var category = await _context.Categories.FindAsync(productDto.CategoryId);

            if (stock == null || category == null)
                return BadRequest("Stock or Category not found.");

            // Mapowanie ProductDto na Product
            var product = _mapper.Map<Product>(productDto);
            product.Stock = stock;
            product.Category = category;

            var response = await _productService.UpdateAsync(id, product);
            if (response.Success)
            {
                var updatedProductDto = _mapper.Map<ProductDto>(response.Data);
                return Ok(updatedProductDto);
            }
            if (response.Message == "Entity not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }

        // DELETE: api/Products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.DeleteAsync(id);
            if (response.Success)
                return Ok(response.Message);
            if (response.Message == "Entity not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }

        // GET: api/Products/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var response = await _productService.GetProductsByCategoryIdAsync(categoryId);
            if (response.Success)
            {
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(response.Data);
                return Ok(productDtos);
            }
            return StatusCode(500, response.Message);
        }

        // GET: api/Products/{id}/stock
        [HttpGet("{id}/stock")]
        public async Task<IActionResult> GetProductWithStock(int id)
        {
            var response = await _productService.GetProductWithStockAsync(id);
            if (response.Success)
            {
                var productDto = _mapper.Map<ProductDto>(response.Data);
                return Ok(productDto);
            }
            if (response.Message == "Product not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }
    }
}
