using System.Net.Mime;
using AutoMapper;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;

namespace main_service.Models;

/// <summary>
/// This is not certain we will use, but this is how it would be used if so
/// This is similar to the Java model mapper, where we could convert a model into a dto
/// without having to manually map each field
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // DTO
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<UserDetails, UserDetailsDto>().ReverseMap();
    }
}