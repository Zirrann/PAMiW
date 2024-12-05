using Shared.Models;
using Shared.Models.Dto;
using Shared.Services;

namespace Shop.MAUI.Services.ServicesDto
{
    public interface IStockServiceDto : ICrudService<StockDto, int>
    {
    }
}
