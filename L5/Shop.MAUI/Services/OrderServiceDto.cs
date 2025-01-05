using Shared.Models.Dto;
using Shop.MAUI.Services.ServicesDto;


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
