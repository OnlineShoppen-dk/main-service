﻿using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.ProductApiModels;
using main_service.Models.DomainModels;
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

    // Sync Product to RabbitMQ
    [HttpPost]
    [Route("sync")]
    public async Task<IActionResult> Sync()
    {
        var products = await _dbContext.Products
            .Include(p => p.Images)
            .Include(p => p.Categories)
            .ToListAsync();

        var productDto = _mapper.Map<List<ProductDto>>(products);
        var serializedProducts = productDto.Select(p => p.ToRepresentation());
        _rabbitMqProducer.SyncProductQueue(serializedProducts);
        return Ok("Products synced to RabbitMQ");
    }

    // Product State Operations
    // These operation are manual operations that can be performed on a product by an admin if needed
    [HttpPost]
    [Route("{productId:int}/sell/{quantity:int}")]
    public async Task<IActionResult> Sell(int productId, int quantity)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null) return NotFound("Product not found");
        product.ProductSale(quantity);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishUpdateProductQueue(deserializeProduct);
        
        return Ok("Product sold");
    }

    [HttpPost]
    [Route("{productId:int}/restock/{quantity:int}")]
    public async Task<IActionResult> Restock(int productId, int quantity)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null) return NotFound("Product not found");
        product.ProductStockUpdate(quantity);
        await _dbContext.SaveChangesAsync();
        
        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishUpdateProductQueue(deserializeProduct);
        
        return Ok("Product restocked");
    }

    // Product Category Operations
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
        _rabbitMqProducer.PublishUpdateProductQueue(deserializeProduct);

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
        _rabbitMqProducer.PublishUpdateProductQueue(deserializeProduct);

        return Ok("Category removed from product");
    }

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
        _rabbitMqProducer.PublishUpdateProductQueue(deserializeProduct);

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
        _rabbitMqProducer.PublishUpdateProductQueue(deserializeProduct);

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
        [FromQuery] string? category
    )
    {
        var products = _dbContext.Products
            .Include(p => p.Images)
            .AsSplitQuery()
            .AsQueryable();
        // Search
        if (search != null)
        {
            products = products.Where(x => x.Name.Contains(search));
        }

        // Category
        if (category != null)
        {
            products = products.Where(x => x.Categories.Any(c => c.Name == category));
        }


        // Sorting
        if (sort != null)
        {
            products = sort switch
            {
                "popularity_asc" => products.OrderBy(p => p.Sold),
                "popularity_desc" => products.OrderByDescending(p => p.Sold),
                "name_asc" => products.OrderBy(p => p.Name),
                "name_desc" => products.OrderByDescending(p => p.Name),
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "stock_asc" => products.OrderBy(p => p.Stock),
                "stock_desc" => products.OrderByDescending(p => p.Stock),
                _ => products.OrderBy(p => p.Id)
            };
        }

        var productTest = await products.ToListAsync();
        Console.WriteLine(productTest.Count);
        // Pagination
        (products, var pageResult, var pageSizeResult, var totalPages, var totalProducts) =
            _paginationService.ApplyPagination(products, page, pageSize);
        // Create response
        var productList = await products.ToListAsync();
        var response = new GetProductsResponse
        {
            TotalProducts = totalProducts,
            Page = pageResult,
            PageSize = pageSizeResult,
            TotalPages = totalPages,
            Search = search ?? "",
            Sort = sort ?? "",
            Products = _mapper.Map<List<ProductDto>>(productList),
        };
        return Ok(response);
    }

    // Get Product by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _dbContext.Products
            .Include(p => p.Images)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        var productDto = _mapper.Map<ProductDto>(product);
        return Ok(productDto);
    }

    // Create Product
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            Sold = 0
        };

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        
        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishProductQueue(deserializeProduct);

        return Ok(product);
    }

    // Update Product
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PutProductRequest request)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        product.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;
        product.Price = request.Price ?? product.Price;
        product.Stock = request.Stock ?? product.Stock;
        product.Sold = request.Sold ?? product.Sold;
        product.Disabled = request.Disabled ?? product.Disabled;
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishUpdateProductQueue(deserializeProduct);

        return Ok(product);
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

        _rabbitMqProducer.PublishRemoveProductQueue(product);
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();

        // Publish update to RabbitMQ
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishUpdateProductQueue(deserializeProduct);

        return Ok("Product deleted");
    }


    public ProductController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService,
        IRabbitMQProducer rabbitMqProducer, IBlobService blobService) : base(mapper, dbContext, paginationService,
        rabbitMqProducer)
    {
        _blobService = blobService;
    }
}