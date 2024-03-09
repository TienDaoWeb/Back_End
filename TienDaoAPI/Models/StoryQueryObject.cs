using TienDaoAPI.Models.Interface;

namespace TienDaoAPI.Models
{
    public class StoryQueryObject : IQueryObject
    {
        public string? Keyword { get; set; }
        public string? SortBy { get; set; }
        public bool IsSortAscending { get; set; }
    }
}
