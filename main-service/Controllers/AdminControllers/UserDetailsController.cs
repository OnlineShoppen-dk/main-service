using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.UserDetailApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class UserDetailController : BaseAdminController
{
    // Get All UserDetails
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? search,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort
    )
    {
        var userDetails = _dbContext.UserDetails
            .AsSplitQuery()
            .AsQueryable();

        // Search
        if(search != null)
        {
            userDetails = userDetails.Where(
                x => x.FirstName.Contains(search) || 
                         x.LastName.Contains(search) || 
                         x.Email.Contains(search) || 
                         x.PhoneNumber.Contains(search));
        }

        // Sorting
        userDetails = sort switch
        {
            "first_name_asc" => userDetails.OrderBy(x => x.FirstName),
            "last_name_desc" => userDetails.OrderByDescending(x => x.LastName),
            _ => userDetails.OrderBy(x => x.Id)
        };

        // Pagination
        (userDetails, var pageResult, var pageSizeResult, var totalPages, var totalUserDetails) =
            _paginationService.ApplyPagination(userDetails, page, pageSize);

        // Create response
        var userDetailList = await userDetails.ToListAsync();

        var response = new GetUserDetailsResponse
        {
            TotalUserDetails = totalUserDetails,
            Page = pageResult,
            PageSize = pageSizeResult,
            TotalPages = totalPages,
            Search = search ?? "",
            Sort = sort ?? "",
            UserDetails = _mapper.Map<List<UserDetailsDto>>(userDetailList),
        };

        return Ok(response);
    }

    // Get UserDetail by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var userDetail = await _dbContext.UserDetails.FindAsync(id);
        if (userDetail == null)
        {
            return NotFound("UserDetail not found");
        }

        return Ok(userDetail);
    }

    // Create UserDetail
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostUserDetailRequest request)
    {
        var userDetails = new UserDetails
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        await _dbContext.UserDetails.AddAsync(userDetails);
        await _dbContext.SaveChangesAsync();
        return Ok(userDetails);
    }

    // Update UserDetail
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PutUserDetailRequest request)
    {
        var userDetail = await _dbContext.UserDetails.FindAsync(id);
        if (userDetail == null)
        {
            return NotFound("UserDetail not found");
        }

        userDetail.FirstName = request.FirstName ?? userDetail.FirstName;
        userDetail.LastName = request.LastName ?? userDetail.LastName;
        userDetail.Email = request.Email ?? userDetail.Email;
        userDetail.PhoneNumber = request.PhoneNumber ?? userDetail.PhoneNumber;
        
        await _dbContext.SaveChangesAsync();
        return Ok(userDetail);
    }

    // Delete UserDetail
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userDetail = await _dbContext.UserDetails.FindAsync(id);
        if (userDetail == null)
        {
            return NotFound("UserDetail not found");
        }

        _dbContext.UserDetails.Remove(userDetail);
        await _dbContext.SaveChangesAsync();
        return Ok("UserDetail deleted");
    }


    public UserDetailController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}