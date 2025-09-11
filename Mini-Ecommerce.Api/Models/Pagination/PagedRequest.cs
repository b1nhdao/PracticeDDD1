namespace Mini_Ecommerce.Api.Models.Pagination
{
    public class PagedRequest
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string Keyword { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = false;
    }
}
