using TienDaoAPI.DTOs;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface ICommentService
    {
        public Task<bool> CreateCommentAsync(CreateCommentDTO comment);
        public Task<bool> DeleteCommentAsync(int commentId);
        public Task<bool> UpdateCommentAsync(Comment comment, UpdateCommentDTO dto);
        public Task<Comment?> GetCommentbyIdAsync(int id);
        public Task<IEnumerable<Comment>?> GetAllCommentAsync(CommentFilter filter);
        public Task<bool> ReplyComment(CreateReplyCommentDTO dto);
        //public Task<ReactionEnum?> UserLikeComment(int commentId, int userId);
    }
}
