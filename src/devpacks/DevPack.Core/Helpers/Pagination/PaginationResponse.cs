namespace DevPack.Core.Helpers.Pagination;

public sealed class PaginationResponse<T> : List<T>, IPaginationResponse
{
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    
    private PaginationResponse(IQueryable<T> items, int totalCount, int currentPage, int pageSize)
    {
        CurrentPage = currentPage;
        TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
        PageSize = pageSize;
        TotalCount = totalCount;
        
        AddRange(items);
    }
    
    private PaginationResponse(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
    {
        CurrentPage = currentPage;
        TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
        PageSize = pageSize;
        TotalCount = totalCount;
        
        AddRange(items);
    }

    public static PaginationResponse<T> ToPagination(IQueryable<T> items, int totalCount, int currentPage, int pageSize) =>
        new(items, totalCount, currentPage, pageSize);
    
    public static PaginationResponse<T> ToPagination(IEnumerable<T> items, int totalCount, int currentPage, int pageSize) =>
        new(items, totalCount, currentPage, pageSize);
}