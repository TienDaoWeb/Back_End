namespace TienDaoAPI.Models
{
    public class ReviewFilter : PaginationFilter
    {
        public ReviewFilter() : base(1, 10)
        {

        }
        public ReviewFilter(int page, int pageSize) : base(page, pageSize)
        {

        }
        public string? SortBy {get;set;}
    }
}
