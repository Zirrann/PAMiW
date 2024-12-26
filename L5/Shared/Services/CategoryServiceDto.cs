using Shared.Models.Dto;
using Shared.Services.Dto;

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
