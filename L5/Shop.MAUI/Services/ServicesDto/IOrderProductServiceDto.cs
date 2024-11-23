using Shared.Models;
using Shared.Models.Dto;
using Shared.Services;

namespace Shop.MAUI.Services.ServicesDto
{
    public interface IOrderProductServiceDto : ICrudService<OrderProductDto, OrderProductKey>
    {
    }
}
