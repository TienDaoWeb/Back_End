using TienDaoAPI.DTOs.Comments;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface ICommentService
    {
        public Task<bool> CreateCommentAsync(CreateCommentDTO comment);
        public Task<bool> DeleteCommentAsync(int commentId);
        public Task<bool> UpdateCommentAsync(Comment comment, UpdateCommentDTO dto);
        public Task<Comment?> GetCommentbyIdAsync(int id);
        public Task<IEnumerable<Comment>?> GetAllCommentsByBookIdAsync(int bookId, CommentFilter filter);
        public Task<bool> ReplyComment(Comment comment, CreateReplyCommentDTO dto);
        public Task<ReactionEnum> LikeOrUnlikeComment(int commentId, int userId);
    }
}
