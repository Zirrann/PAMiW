using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;
using Shared.Models.Dto;

namespace Shop.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : CrudController<Stock, StockDto, int>
    {
        public StockController(IStockService service, IMapper mapper)
            : base(service, mapper)
        {
        }
    }
}
