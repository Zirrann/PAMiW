using Shared.Models.Dto;
using Shared.Services;
using Shop.WPF.Services.ServicesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shop.WPF.Services
{
    public class OrderServiceDto : CrudServiceDto<OrderDto, int>, IOrderServiceDto
    {
        public OrderServiceDto(HttpClient httpClient)
            : base(httpClient, "api/Order")
        {
        }
    }
}
