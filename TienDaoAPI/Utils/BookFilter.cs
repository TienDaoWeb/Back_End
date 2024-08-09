using TienDaoAPI.Enums;

namespace TienDaoAPI.Utils
{
    public class BookFilter : PaginationFilter
    {
        public BookFilter() : base(1, 20)
        {

        }
        public BookFilter(int page, int pageSize) : base(page, pageSize)
        {

        }
        public string? Keyword { get; set; }
        public string? Include { get; set; }
        public BookStatusEnum? Status { get; set; }
        public string? Genres { get; set; }
        public string? Tags { get; set; }
        public string? Chapter { get; set; }
        public string? SortBy { get; set; }

    }
}
