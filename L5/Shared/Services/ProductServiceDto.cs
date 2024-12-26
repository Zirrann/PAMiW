using Shared.Models.Dto;


namespace Shared.Services
{
    public class ProductServiceDto : CrudServiceDto<ProductDto, int>, IProductServiceDto
    {
        public ProductServiceDto(HttpClient httpClient) 
            : base(httpClient, "api/Products")
        {
        }
    }
}
