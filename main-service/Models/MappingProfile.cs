using System.Net.Mime;
using AutoMapper;
using main_service.Models.DomainModels;
using main_service.Models.DomainModels.ProductDomainModels;
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
        // Product & ProductDescription -> ProductDto
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductDescription.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDescription.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductDescription.Price))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
            .ForMember(dest => dest.Sold, opt => opt.MapFrom(src => src.Sold))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.IsRemoved, opt => opt.MapFrom(src => src.IsRemoved))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.ProductDescription.UpdatedAt))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => new CategoryDto
            {
                /* mapping for CategoryDto */
            }).ToList()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => new ImageDto
            {
                /* mapping for ImageDto */
            }).ToList()));
        CreateMap<ProductDescription, ProductDescriptionDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<UserDetails, UserDetailsDto>().ReverseMap();
        CreateMap<Image, ImageDto>().ReverseMap();
    }
}