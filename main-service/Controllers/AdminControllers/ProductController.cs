using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.ProductApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DomainModels.ProductDomainModels;
using main_service.Models.DtoModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class ProductController : BaseAdminController
{
    private readonly IBlobService _blobService;

    // Product Category Operations
    [HttpPost]
    [Route("{productId:int}/add-image")]
    public async Task<IActionResult> AddImage(int productId, IFormFile file)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null) return NotFound("Product not found");
        var newFileName = $"{Guid.NewGuid()}_{Path.GetExtension(file.FileName)}_";

        while (await _blobService.ImageExists(newFileName))
        {
            newFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        }

        var (status, message) = await _blobService.UploadImage(newFileName, file);
        if (status != 1) return BadRequest(message);
        // Filename minus file extension
        var image = new Image
        {
            Name = Path.GetFileNameWithoutExtension(file.FileName),
            FileName = message,
            Alt = "image",
        };
        product.Images.Add(image);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishProductQueue(deserializeProduct);

        return Ok("Image added to product");
    }

    [HttpDelete]
    [Route("{productId:int}/delete-image/{imageId:int}")]
    public async Task<IActionResult> DeleteImage(int productId, int imageId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null) return NotFound("Product not found");

        var image = await _dbContext.Images.FindAsync(imageId);
        if (image == null) return NotFound("Image not found");

        await _blobService.DeleteImage(image.FileName);
        product.Images.Remove(image);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishProductQueue(deserializeProduct);

        return Ok("Image deleted");
    }

    // BASIC CRUD OPERATIONS
    // Get All Products
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? search,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort,
        [FromQuery] bool? includeRemoved
    )
    {
        var products = _dbContext.Products
            .Include(p => p.Images)
            .Include(p => p.Categories)
            .Include(p => p.ProductDescriptions)
            .Include(p => p.ProductRemoved)
            .AsSplitQuery()
            .OrderBy(p => p.Id)
            .AsQueryable();
        
        // Include Removed
        if (includeRemoved != true)
        {
            products = products.Where(p => p.ProductRemoved == null);
        }
        
        // Search
        if (search != null)
        {
            products = products.Where(p => p.ProductDescriptions.Any(pd => pd.Name.Contains(search)));
        }
        
        if(!products.Any()) return NotFound("No products found");
        

        // Sorting
        // Products will always have a product description, so we can safely use FirstOrDefault here
        if (sort != null)
        {
            products = sort switch
            {
                "popularity_asc" => products.OrderBy(p => p.Sold),
                "popularity_desc" => products.OrderByDescending(p => p.Sold),
                "name_asc" => products.OrderBy(p => p.ProductDescriptions.OrderByDescending(pd => pd.UpdatedAt).FirstOrDefault()!.Name),
                "name_desc" => products.OrderByDescending(p => p.ProductDescriptions.OrderByDescending(pd => pd.UpdatedAt).FirstOrDefault()!.Name),
                "price_asc" => products.OrderBy(p => p.ProductDescriptions.OrderByDescending(pd => pd.UpdatedAt).FirstOrDefault()!.Price),
                "price_desc" => products.OrderByDescending(p => p.ProductDescriptions.OrderByDescending(pd => pd.UpdatedAt).FirstOrDefault()!.Price),
                "stock_asc" => products.OrderBy(p => p.Stock),
                "stock_desc" => products.OrderByDescending(p => p.Stock),
                _ => products.OrderBy(p => p.Id)
            };
        }
        

        // Pagination
        (products, var pageResult, var pageSizeResult, var totalPages, var totalProducts) =
            _paginationService.ApplyPagination(products, page, pageSize);

        // Get the products
        var productList = await products.ToListAsync();

        // Map to DTO
        var productDtoList = _mapper.Map<List<ProductDto>>(productList);
        
        var response = new GetProductsResponse
        {
            TotalProducts = totalProducts,
            Page = pageResult,
            PageSize = pageSizeResult,
            TotalPages = totalPages,
            Search = search ?? "",
            Sort = sort ?? "",
            IncludeRemoved = includeRemoved ?? false,
            Products = productDtoList,
        };
        return Ok(response);
    }

    // Get Product by ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _dbContext.Products
            .Include(p => p.ProductDescriptions)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound("Product not found");
        }
        
        var productHistory = product.ProductDescriptions.Select(pd
            => _mapper.Map<ProductDescriptionDto>(pd)).ToList();
        
        // Sort ProductHistory by UpdatedAt, så the latest update is first
        productHistory.Sort((x, y) => y.UpdatedAt.CompareTo(x.UpdatedAt));
        var productDto = _mapper.Map<ProductDto>(product);

        var response = new
        {
            product = productDto,
            productHistory = productHistory
        };
        return Ok(response);
    }

    // Create Product
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostProductRequest request)
    {
        var product = new Product
        {
            Guid = request.Guid,
            Stock = request.Stock,
            Sold = request.Sold,
        };
        // Create first instance of ProductDescription
        var productDescription = new ProductDescription
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };
        product.ProductDescriptions.Add(productDescription);
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        PublishProductToBroker(product);
        var productDto = _mapper.Map<ProductDto>(product);
        return Ok(productDto);
    }

    // Update Product
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] PutProductRequest request)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }
        var productDescription = new ProductDescription
        {
            Name = request.Name ?? product.ProductDescription.Name,
            Description = request.Description ?? product.ProductDescription.Description,
            Price = request.Price ?? product.ProductDescription.Price,
            
        };
        product.Stock = request.Stock ?? product.Stock;
        product.Sold = request.Sold ?? product.Sold;
        product.ProductDescriptions.Add(productDescription);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        PublishProductToBroker(product);
        
        // Response
        var productDto = _mapper.Map<ProductDto>(product);
        return Ok(productDto);
    }
    
    // Active Product / Restore Product
    [HttpPut("{id:int}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var product = await _dbContext.Products.Include(p => p.ProductRemoved).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound("Product not found");
        }
        product.ProductRemoved = null;
        await _dbContext.SaveChangesAsync();
        // Publish update to RabbitMQ
        PublishProductToBroker(product);
        return Ok("Product restored");
    }
    // Delete Product
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }
        var productRemoved = new ProductRemoved
        {
            ProductId = product.Id,
            RemovedAt = DateTimeOffset.Now,
        };
        product.ProductRemoved = productRemoved;
        
        // _rabbitMqProducer.PublishRemoveProductQueue(product);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        PublishProductToBroker(product);
        
        return Ok("Product deleted");
    }

    
    private void PublishProductToBroker(Product product)
    {
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishProductQueue(deserializeProduct);
    }

    public ProductController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService,
        IRabbitMQProducer rabbitMqProducer, IBlobService blobService) : base(mapper, dbContext, paginationService,
        rabbitMqProducer)
    {
        _blobService = blobService;
    }
}

/* OLD FUNCTIONS
[HttpPost]
    [Route("{productId:int}/add-category/{categoryId:int}")]
    public async Task<IActionResult> AddCategory(int productId, int categoryId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null) return NotFound("Product not found");

        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category == null) return NotFound("Category not found");

        product.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishProductQueue(deserializeProduct);

        return Ok("Category added to product");
    }

    [HttpPost]
    [Route("{productId:int}/remove-category/{categoryId:int}")]
    public async Task<IActionResult> RemoveCategory(int productId, int categoryId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null) return NotFound("Product not found");

        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category == null) return NotFound("Category not found");

        product.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishProductQueue(deserializeProduct);
        return Ok("Category removed from product");
    }

 */