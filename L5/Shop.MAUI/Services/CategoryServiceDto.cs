using Shared.Models;
using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;

namespace Shop.MAUI.Services
{
    public class CategoryServiceDto : CrudServiceDto<CategoryDto, int>, ICategoryServiceDto
    {
        public CategoryServiceDto(HttpClient httpClient)
            : base(httpClient, "api/Categoty")
        {
        }
    }
}
