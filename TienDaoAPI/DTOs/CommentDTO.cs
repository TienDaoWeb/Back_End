namespace TienDaoAPI.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int BookId { get; set; }
        public int LikeCount { get; set; }
        public ChapterShortDTO? Chapter { get; set; }
        public UserBaseDTO? Owner { get; set; }
        public List<CommentReplyDTO> CommentReplies { get; set; } = new List<CommentReplyDTO>();
    }
}
