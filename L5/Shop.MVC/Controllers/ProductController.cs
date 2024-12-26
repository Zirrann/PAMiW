using L4.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;
using Shop.MVC.Models;

namespace Shop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServiceDto _productServiceDto;
        private readonly ICategoryServiceDto _categoryServiceDto;
        private IEnumerable<CategoryDto> categories;


        public ProductController(
            IProductServiceDto productServiceDto,
            ICategoryServiceDto categoryServiceDto)
        {
            _productServiceDto = productServiceDto;
            _categoryServiceDto = categoryServiceDto;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productServiceDto.GetAllAsync();
            var categoriesResponse = await _categoryServiceDto.GetAllAsync();

            if (response.Success)
            {
                if (categoriesResponse.Success)
                {
                    categories = categoriesResponse.Data;
                    ViewBag.Categories = categories;  
                }
                else
                {
                    ViewBag.Categories = null;  
                }

                return View(response.Data);
            }

            ViewBag.Error = "Failed to load products.";
            return View(new List<ProductDto>());
        }


        public async Task<IActionResult> Details(int id)
        {
            var productResponse = await _productServiceDto.GetByIdAsync(id);
            var categoriesResponse = await _categoryServiceDto.GetAllAsync();

            if (productResponse.Success && categoriesResponse.Success)
            {
                var model = new ProductDetailsModel
                {
                    Product = productResponse.Data,
                    Categories = categoriesResponse.Data
                };
                return View(model);
            }

            ViewBag.Error = "Product or categories not found.";
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
        public async Task<IActionResult> Add(ProductDto product, int quantity)
        {
            if (ModelState.IsValid)
            {
                    product.Quantity = quantity;

                    var productResponse = await _productServiceDto.CreateAsync(product);

                    if (productResponse.Success)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    ViewBag.Error = productResponse.Message;
                
            }

            return View("Index", await _productServiceDto.GetAllAsync());
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                // Aktualizacja produktu w bazie danych
                var response = await _productServiceDto.UpdateAsync(product.Id, product);

                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));  // Po udanej aktualizacji przekierowujesz na stronę z listą produktów
                }

                ViewBag.Error = response.Message;  // Jeśli aktualizacja nie powiedzie się, wyświetl błąd
            }

            // Jeśli formularz nie jest poprawny, ponownie ładujemy kategorie
            var categoriesResponse = await _categoryServiceDto.GetAllAsync();
            ViewBag.Categories = categoriesResponse.Success ? categoriesResponse.Data : new List<CategoryDto>();

            // Tworzymy model z aktualnymi danymi i kategoriami
            var model = new ProductDetailsModel
            {
                Product = product,
                Categories = ViewBag.Categories as List<CategoryDto>
            };

            return View("Details", model);  // Zwracamy widok "Details" z modelami
        }


    }
}
