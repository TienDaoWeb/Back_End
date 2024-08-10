using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface ICommentService
    {
        public Task<Comment?> CreateCommentAsync(CreateCommentDTO comment);
        public Task<bool> DeleteCommentAsync(int commentId);
        public Task<Comment?> UpdateCommentAsync(Comment comment , UpdateCommentDTO dto);
        public Task<Comment?> GetCommentbyIdAsync(int id);
        public Task<IEnumerable<Comment?>> GetAllCommentAsync(CommentFilter filter);
        public Task<Comment?> ReplyComment(CreateReplyCommentDTO dto);
        public Task<ReactionEnum?> UserLikeComment(int commentId, int userId);
    }
}
