namespace Mini_Ecommerce.Api.Models.Pagination
{
    public class PagedResponse<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T>? Data { get; set; }
        public int TotalPages => TotalCount / PageSize;
        public bool HasPreviousPage => PageIndex > 0;
        public bool HasNextPage => PageIndex < TotalPages;

        public PagedResponse(int pageIndex, int pageSize, int totalCount, IEnumerable<T>? data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }
    }
}
