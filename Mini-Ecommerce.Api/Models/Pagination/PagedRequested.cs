namespace Mini_Ecommerce.Api.Models.Pagination
{
    public class PagedRequested
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public PagedRequested(int pageIndex = 0, int pageSize = 10)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string? Search { get; set; }
        public string? SortBy { get; set; }

        public bool SortDeceding = false;
    }
}
