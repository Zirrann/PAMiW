using Shared.Models.Dto;
using Shop.WPF.Services.ServicesDto;
using System.Net.Http;


namespace Shop.WPF.Services
{
    public class ProductServiceDto : CrudServiceDto<ProductDto, int>, IProductServiceDto
    {
        public ProductServiceDto(HttpClient httpClient) 
            : base(httpClient, "api/Products")
        {
        }
    }
}
