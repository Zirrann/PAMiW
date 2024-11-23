using AutoMapper;
using Shared.Models;
using Shared.Models.Dto;

namespace Shop.MAUI;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<StockDto, StockDto>().ReverseMap();
        CreateMap<OrderDto, OrderDto>().ReverseMap();
        CreateMap<OrderProduct, OrderProductDto>().ReverseMap();
    }
}
