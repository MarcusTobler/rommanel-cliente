namespace DevPack.Core.Helpers.Pagination;

public interface IPaginationResponse
{
    int CurrentPage { get; }
    int TotalPages { get; }
    int PageSize { get; }
    int TotalCount { get; }

    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}