using L4.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;

namespace Shop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServiceDto _orderServiceDto;

        public OrderController(IOrderServiceDto orderServiceDto)
        {
            _orderServiceDto = orderServiceDto;
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
            var response = await _orderServiceDto.GetByIdAsync(id);
            if (response.Success)
            {
                return View(response.Data);
            }

            ViewBag.Error = "Order not found.";
            return RedirectToAction(nameof(Index));
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
