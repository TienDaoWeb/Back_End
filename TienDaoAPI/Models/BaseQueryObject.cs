namespace TienDaoAPI.Models
{
    public class BaseQueryObject
    {
        public string? Keyword { get; set; }
        public string? SortBy { get; set; }
        public bool IsSortAscending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
