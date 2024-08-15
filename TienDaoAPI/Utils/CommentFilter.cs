using TienDaoAPI.Utils;

namespace TienDaoAPI.Models
{
    public class CommentFilter : PaginationFilter
    {
        public CommentFilter() : base(1, 5)
        {

        }
        public CommentFilter(int page, int pageSize) : base(page, pageSize)
        {

        }
        public string? SortBy { get; set; }
    }
}
