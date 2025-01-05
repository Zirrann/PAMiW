using Shared.Models.Dto;
using Shared.Services;
using Shop.BLZR.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLZR.Services
{
    public class OrderServiceDto : CrudServiceDto<OrderDto, int>, IOrderServiceDto
    {
        public OrderServiceDto(HttpClient httpClient)
            : base(httpClient, "api/Order")
        {
        }
    }
}
