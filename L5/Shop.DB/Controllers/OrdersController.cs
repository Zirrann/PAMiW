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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllAsync();
            if (response.Success)
            {
                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(response.Data);
                return Ok(orderDtos);
            }
            return StatusCode(500, response.Message);
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var response = await _orderService.GetOrderWithOrderProductsByIdAsync(id);
            if (response.Success)
            {
                var orderDto = _mapper.Map<OrderDto>(response.Data);
                return Ok(orderDto);
            }
            if (response.Message == "Order not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
                return BadRequest("Order cannot be null.");

            var order = _mapper.Map<Order>(orderDto);
            var response = await _orderService.CreateAsync(order);

            if (response.Success)
            {
                var createdOrderDto = _mapper.Map<OrderDto>(response.Data);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrderDto.OrderId }, createdOrderDto);
            }

            return StatusCode(500, response.Message);
        }

        // PUT: api/Orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            if (orderDto == null || orderDto.OrderId != id)
                return BadRequest("Order data is invalid.");

            var order = _mapper.Map<Order>(orderDto);
            var response = await _orderService.UpdateAsync(id, order);

            if (response.Success)
            {
                var updatedOrderDto = _mapper.Map<OrderDto>(response.Data);
                return Ok(updatedOrderDto);
            }

            if (response.Message == "Entity not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _orderService.DeleteAsync(id);
            if (response.Success)
                return Ok(response.Message);
            if (response.Message == "Entity not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }
    }
}
