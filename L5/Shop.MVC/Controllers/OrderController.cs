using L4.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;
using Shop.MVC.Models;

namespace Shop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServiceDto _orderServiceDto;
        private readonly IProductServiceDto _productServiceDto;
        private readonly IOrderProductServiceDto _orderProductServiceDto;

        public OrderController(
            IOrderServiceDto orderServiceDto,
            IProductServiceDto productServiceDto,
            IOrderProductServiceDto orderProductServiceDto)
        {
            _orderServiceDto = orderServiceDto;
            _productServiceDto = productServiceDto;
            _orderProductServiceDto = orderProductServiceDto;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _orderServiceDto.GetAllAsync();
            if (response.Success)
            {
                return View(response.Data);
            }

            ViewBag.Error = "Failed to load orders.";
            return View(new List<OrderDto>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var orderResponse = await _orderServiceDto.GetByIdAsync(id);
            var productsResponse = await _productServiceDto.GetAllAsync();

            if (orderResponse.Success && productsResponse.Success)
            {
                var order = orderResponse.Data;
                var orderProductIds = order.OrderProducts.Select(op => op.ProductId).ToHashSet();

                var viewModel = new OrderDetailsModel
                {
                    Order = order,
                    AvailableProducts = productsResponse.Data
                        .Where(p => !orderProductIds.Contains(p.Id))    
                        .ToList(),
                    SelectedProducts = productsResponse.Data
                        .Where(p => orderProductIds.Contains(p.Id))
                        .ToList()
                };

                return View(viewModel);
            }

            ViewBag.Error = "Failed to load order details.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(int orderId, int productId)
        {
            var response = await _orderProductServiceDto.CreateAsync(new OrderProductDto
            {
                OrderId = orderId,
                ProductId = productId
            });

            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveProduct(int orderId, int productId)
        {
            var response = await _orderProductServiceDto.DeleteAsync(new OrderProductKey(productId, orderId));
            return RedirectToAction(nameof(Details), new { id = orderId });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _orderServiceDto.DeleteAsync(id);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
