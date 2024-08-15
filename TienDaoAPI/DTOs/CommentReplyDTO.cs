namespace TienDaoAPI.DTOs
{
    public class CommentReplyDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int LikeCount { get; set; }
        public UserBaseDTO? Owner { get; set; }
    }
}
