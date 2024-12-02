using Shared.Models;
using Shared.Models.Dto;
using Shop.WPF.Services.ServicesDto;
using System.Net.Http;

namespace Shop.WPF.Services
{
    public class CategoryServiceDto : CrudServiceDto<CategoryDto, int>, ICategoryServiceDto
    {
        public CategoryServiceDto(HttpClient httpClient)
            : base(httpClient, "api/Categoty")
        {
        }
    }
}
