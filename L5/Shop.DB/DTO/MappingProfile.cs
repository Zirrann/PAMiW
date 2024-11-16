using AutoMapper;
using Shared.Models;

namespace Shop.DB.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap()
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))  // Mapowanie Stock
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));  // Mapowanie Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Stock, StockDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderProduct, OrderProductDto>().ReverseMap();
        }
    }
}
