using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
