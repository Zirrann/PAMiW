using Shared.Models.Dto;
using Shop.BLZR.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLZR.Services
{
    public class ProductServiceDto : CrudServiceDto<ProductDto, int?>, IProductServiceDto
    {
        public ProductServiceDto(HttpClient httpClient) 
            : base(httpClient, "api/Products")
        {
        }
    }
}
