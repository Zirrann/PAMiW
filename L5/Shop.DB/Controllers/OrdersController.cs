using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;
using Shop.DB.DTO;

namespace Shop.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CrudController<Order, OrderDto, int>
    {
        public OrderController(IOrderService service, IMapper mapper)
            : base(service, mapper)
        {
        }
    }
}
