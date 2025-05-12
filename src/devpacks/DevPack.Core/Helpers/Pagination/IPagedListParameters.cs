namespace DevPack.Core.Helpers.Pagination
{
    public interface IPagedListParameters
    {
        int CurrentPage { get; set; }
        int PageSize { get; }
    }
}