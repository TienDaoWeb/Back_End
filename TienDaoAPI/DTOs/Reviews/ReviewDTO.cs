using TienDaoAPI.DTOs.Users;

namespace TienDaoAPI.DTOs.Reviews
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public float Score { get; set; }
        public string? Content { get; set; }
        public int LikeCount { get; set; }
        public UserBaseDTO? Owner { get; set; }
        public int BookId { get; set; } = 0;
        public DateTime CreateAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
