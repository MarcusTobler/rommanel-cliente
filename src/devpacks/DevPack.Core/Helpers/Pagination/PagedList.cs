namespace DevPack.Core.Helpers.Pagination;

public record PagedList<T>
{
    public static PaginationResponse<T> HasPagination(IQueryable<T> items, int currentPage, int pageSize) =>
        PaginationResponse<T>.ToPagination(items.Skip((currentPage - 1) * pageSize).Take(pageSize),
            items.Count(), 
            currentPage, 
            pageSize);

    public static PaginationResponse<T> HasPagination(IQueryable<T> items, int totalCount, int currentPage, int pageSize) =>
        PaginationResponse<T>.ToPagination(items, totalCount, currentPage, pageSize);

    public static PaginationResponse<T> HasPagination(ICollection<T> items, int currentPage, int pageSize) =>
        PaginationResponse<T>.ToPagination(items.Skip((currentPage - 1) * pageSize).Take(pageSize),
            items.Count, 
            currentPage, 
            pageSize);
    
    public static PaginationResponse<T> HasPagination(ICollection<T> items, int totalCount, int currentPage, int pageSize) =>
        PaginationResponse<T>.ToPagination(items, totalCount, currentPage, pageSize);
}