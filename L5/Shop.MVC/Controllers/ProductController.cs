using L4.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;

namespace Shop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServiceDto _productServiceDto;
        private readonly ICategoryServiceDto _categoryServiceDto;
        private readonly IStockServiceDto _stockServiceDto;

        public ProductController(
            IProductServiceDto productServiceDto,
            ICategoryServiceDto categoryServiceDto,
            IStockServiceDto stockServiceDto)
        {
            _productServiceDto = productServiceDto;
            _categoryServiceDto = categoryServiceDto;
            _stockServiceDto = stockServiceDto;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productServiceDto.GetAllAsync();
            var categoriesResponse = await _categoryServiceDto.GetAllAsync();

            if (response.Success)
            {
                if (categoriesResponse.Success)
                {
                    ViewBag.Categories = categoriesResponse.Data;  // Przekazanie listy kategorii do widoku
                }
                else
                {
                    ViewBag.Categories = null;  // W przypadku błędu pobierania kategorii
                }

                return View(response.Data);
            }

            ViewBag.Error = "Failed to load products.";
            return View(new List<ProductDto>());
        }


        public async Task<IActionResult> Details(int id)
        {
            var response = await _productServiceDto.GetByIdAsync(id);
            if (response.Success)
            {
                return View(response.Data);
            }

            ViewBag.Error = "Product not found.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productServiceDto.DeleteAsync(id);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductDto product, int quantity) // Dodajemy quantity jako argument
        {
            if (ModelState.IsValid)
            {
                // Tworzymy nowy StockDto na podstawie quantity
                var newStock = new StockDto
                {
                    Quantity = quantity // Używamy quantity przekazanego z formularza
                };

                // Tworzymy nowy Stock w bazie
                var stockResponse = await _stockServiceDto.CreateAsync(newStock);

                if (stockResponse.Success)
                {
                    // Przypisujemy StockId do produktu
                    product.StockId = stockResponse.Data.StockId;

                    // Tworzymy produkt z nowym StockId
                    var productResponse = await _productServiceDto.CreateAsync(product);

                    if (productResponse.Success)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ViewBag.Error = productResponse.Message;
                }
                else
                {
                    ViewBag.Error = "Failed to create stock.";
                }
            }

            // W przypadku niepowodzenia w dodaniu produktu lub stanu magazynowego, zwróćmy listę produktów
            return View("Index", await _productServiceDto.GetAllAsync());
        }
    }
}
