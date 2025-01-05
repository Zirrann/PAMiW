using Shared.Models;
using Shared.Models.Dto;
using Shop.BLZR.Services.ServicesDto;

namespace Shop.BLZR.Services
{
    public class CategoryServiceDto : CrudServiceDto<CategoryDto, int>, ICategoryServiceDto
    {
        public CategoryServiceDto(HttpClient httpClient)
            : base(httpClient, "api/Categoty")
        {
        }
    }
}
