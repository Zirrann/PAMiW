using Shared.Services;
using Shared.Models.Dto;
using Shop.BLZR.Services.ServicesDto;
using System.Collections.ObjectModel;
using Shop.Tests.services;

namespace Shop.Tests.Fakes
{
    public class FakeProductServiceDto : FakeService<ProductDto, int?>, IProductServiceDto
    {
        public FakeProductServiceDto()
        {
            _dataStore.Add(new ProductDto { Id = 1, Name = "Laptop", Price = 1000, Quantity = 10, CategoryId = 1 });
            _dataStore.Add(new ProductDto { Id = 2, Name = "Phone", Price = 500, Quantity = 20, CategoryId = 2 });
        }

        public override async Task<ServiceReponse<IEnumerable<ProductDto>>> GetAllAsync()
        {
            return await Task.FromResult(new ServiceReponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Data = _dataStore
            });
        }

    }

}
