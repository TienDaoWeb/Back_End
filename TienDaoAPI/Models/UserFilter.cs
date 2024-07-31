namespace TienDaoAPI.Models
{
    public class UserFilter : PaginationFilter
    {
        public UserFilter() : base(1, 20)
        {

        }
        public UserFilter(int page, int pageSize) : base(page, pageSize)
        {

        }

        public string? Keyword { get; set; }
        public string? Status { get; set; }
        public string? SortBy { get; set; }
    }
}
