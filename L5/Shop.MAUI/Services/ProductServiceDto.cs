using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;
using System.Net.Http;


namespace Shop.MAUI.Services
{
    public class ProductServiceDto : CrudServiceDto<ProductDto, int>, IProductServiceDto
    {
        public ProductServiceDto(HttpClient httpClient) 
            : base(httpClient, "api/Products")
        {
        }
    }
}
