namespace ExaminationSystem.Common.Pagination
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResult() { }

        public PaginatedResult(List<T> items, int totalCount, int page, int pageSize, int totalPages)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
            TotalPages = totalPages;
        }
    }
}
