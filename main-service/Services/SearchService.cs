using main_service.Models.DomainModels;

namespace main_service.Services;

public interface IPaginationService
{
    public (IQueryable<T>, int page, int pageSize, int totalPages, int totalItems) ApplyPagination<T>(IQueryable<T> query, int? page, int? pageSize);
}

public class PaginationService : IPaginationService
{
    public (IQueryable<T>, int page, int pageSize, int totalPages, int totalItems) ApplyPagination<T>(IQueryable<T> query, int? page, int? pageSize)
    {
        // Default Page and PageSize values
        const int defaultPage = 1;
        const int defaultPageSize = 25;
        
        // Set default values if not provided
        page ??= defaultPage;
        pageSize ??= defaultPageSize;
        
        // Check if page and pageSize are valid
        if (page < 1) page = defaultPage;
        if (pageSize < 1) pageSize = defaultPageSize;
        
        var pageValue = page.Value;
        var pageSizeValue = pageSize.Value;
        
        // Check total items and total pages
        var totalItems = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize.Value);
        
        // Check if page is greater than total pages
        if (page > totalPages) page = totalPages;
        
        // Skip and Take
        var resultQuery = query.Skip((int)((page - 1) * pageSize)!).Take((int)pageSize);

        // Return the result
        return (
            resultQuery,
            pageValue,
            pageSizeValue,
            totalPages,
            totalItems
        );
    }
}