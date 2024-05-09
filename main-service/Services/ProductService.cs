using AutoMapper;
using main_service.Models.DomainModels.ProductDomainModels;
using main_service.Models.DtoModels;

namespace main_service.Services;

public interface IProductService
{
    public ProductDto ConvertToDto(Product product);
    
}
public class ProductService : IProductService
{
    private readonly IMapper _mapper;

    public ProductDto ConvertToDto(Product product)
    {
        var productDto = product.ConvertToDto(
            _mapper.Map<List<CategoryDto>>(product.Categories),
            _mapper.Map<List<ImageDto>>(product.Images)
        );
        return productDto;
    }
    
    public ProductService(IMapper mapper)
    {
        _mapper = mapper;
    }

}