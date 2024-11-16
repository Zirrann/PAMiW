using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shop.DB.Services;
using Shared.Models;
using Shared.Services;
using Shop.DB.DTO;

namespace Shop.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductsController : ControllerBase
    {
        private readonly IOrderProductService _orderProductService;
        private readonly IMapper _mapper;

        public OrderProductsController(IOrderProductService orderProductService, IMapper mapper)
        {
            _orderProductService = orderProductService;
            _mapper = mapper;
        }

        // GET: api/OrderProducts
        [HttpGet]
        public async Task<IActionResult> GetAllOrderProducts()
        {
            var response = await _orderProductService.GetAllAsync();
            if (response.Success)
            {
                var orderProductDtos = _mapper.Map<IEnumerable<OrderProductDto>>(response.Data);
                return Ok(orderProductDtos);
            }
            return StatusCode(500, response.Message);
        }

        // GET: api/OrderProducts/order/{orderId}
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderProductsByOrderId(int orderId)
        {
            var response = await _orderProductService.GetOrderProductsByOrderIdAsync(orderId);
            if (response.Success)
            {
                var orderProductDtos = _mapper.Map<IEnumerable<OrderProductDto>>(response.Data);
                return Ok(orderProductDtos);
            }
            return StatusCode(500, response.Message);
        }

        // POST: api/OrderProducts
        [HttpPost]
        public async Task<IActionResult> CreateOrderProduct([FromBody] OrderProductDto orderProductDto)
        {
            if (orderProductDto == null)
                return BadRequest("OrderProduct cannot be null.");

            var orderProduct = _mapper.Map<OrderProduct>(orderProductDto);
            var response = await _orderProductService.CreateAsync(orderProduct);

            if (response.Success)
            {
                var createdOrderProductDto = _mapper.Map<OrderProductDto>(response.Data);
                return CreatedAtAction(nameof(GetAllOrderProducts), new { id = createdOrderProductDto.OrderId }, createdOrderProductDto);
            }

            return StatusCode(500, response.Message);
        }

        // DELETE: api/OrderProducts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderProduct(int id)
        {
            var response = await _orderProductService.DeleteAsync(id);
            if (response.Success)
                return Ok(response.Message);
            return StatusCode(500, response.Message);
        }
    }
}
