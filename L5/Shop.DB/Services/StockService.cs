using Shared.Models;
using Shared.Services;

namespace Shop.DB.Services
{
    public class StockService : CrudService<Stock, int>, IStockService
    {
        public StockService(AppDbContext dbContext) : base(dbContext) { }
    }
}
