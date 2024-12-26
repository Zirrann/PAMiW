using Shared.Models.Dto;
using Shared.Services;
using Shop.WPF.Services.ServicesDto;
using System.Net.Http;


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
