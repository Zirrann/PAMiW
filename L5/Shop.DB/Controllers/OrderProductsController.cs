using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;
using Shared.Models.Dto;

namespace Shop.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : CrudController<OrderProduct, OrderProductDto, OrderProductKey>
    {
        public OrderProductController(IOrderProductService service, IMapper mapper)
            : base(service, mapper)
        {
        }

        // GET: api/[controller]/{orderId}/{productId}
        [HttpGet("{orderId}/{productId}")]
        public virtual async Task<IActionResult> GetById(int orderId, int productId)
        {
            var id = new OrderProductKey
            {
                OrderId = orderId,
                ProductId = productId
            };

            var response = await _service.GetByIdAsync(id);
            if (response.Success)
            {
                var dto = _mapper.Map<OrderProductDto>(response.Data);
                return Ok(dto);
            }
            if (response.Data == null)
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }


        // DELETE: api/[controller]/{orderId}/{productId}
        [HttpDelete("{orderId}/{productId}")]
        public virtual async Task<IActionResult> Delete(int orderId, int productId)
        {
            var id = new OrderProductKey
            {
                OrderId = orderId,
                ProductId = productId
            };

            var response = await _service.DeleteAsync(id);
            if (response.Success)
                return Ok(response.Message);
            if (response.Message == "Entity not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }
    }
}
