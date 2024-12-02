using Shared.Models.Dto;
using Shop.WPF.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
