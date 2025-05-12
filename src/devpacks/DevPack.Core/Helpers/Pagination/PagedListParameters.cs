namespace DevPack.Core.Helpers.Pagination
{
    public class PagedListParameters : IPagedListParameters
    {
        private const int MaxPageSize = 1000;

        private int _pageSize = 10;
        public int CurrentPage { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        private PagedListParameters(int currentPage = 1, int pageSize = 10) => 
            (CurrentPage, PageSize) = (currentPage, pageSize);
        
        public static PagedListParameters ToParameters(int pageNumber = 1, int pageSize = 10) => new(pageNumber, pageSize);
    }
}