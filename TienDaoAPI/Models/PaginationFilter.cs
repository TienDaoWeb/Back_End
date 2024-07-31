namespace TienDaoAPI.Models
{
    public class PaginationFilter
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public PaginationFilter()
        {

        }

        public PaginationFilter(int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize > 20 ? 20 : pageSize;
        }
    }
}
