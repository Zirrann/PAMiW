using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;
using Shared.Models.Dto;

namespace Shop.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategotyController : CrudController<Category, CategoryDto, int>
    {
        public CategotyController(ICategoryService service, IMapper mapper)
            : base(service, mapper)
        {
        }
    }
}
