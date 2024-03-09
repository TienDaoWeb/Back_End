namespace TienDaoAPI.Models.Interface
{
    public interface IQueryObject
    {
        public string? Keyword { get; set; }
        public string? SortBy { get; set; }
        public bool IsSortAscending { get; set; }
    }
}
