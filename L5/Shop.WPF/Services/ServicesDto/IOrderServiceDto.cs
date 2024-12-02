using Shared.Models;
using Shared.Models.Dto;
using Shared.Services;

namespace Shop.WPF.Services.ServicesDto
{
    public interface IOrderServiceDto : ICrudService<OrderDto, int>
    {
    }
}
