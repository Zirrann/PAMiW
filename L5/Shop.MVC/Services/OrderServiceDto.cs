using Shared.Models.Dto;
using Shared.Services;
using Shop.MAUI.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.MAUI.Services
{
    public class OrderServiceDto : CrudServiceDto<OrderDto, int>, IOrderServiceDto
    {
        public OrderServiceDto(HttpClient httpClient)
            : base(httpClient, "api/Order")
        {
        }
    }
}
