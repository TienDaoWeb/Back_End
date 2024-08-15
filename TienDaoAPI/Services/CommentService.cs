using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs.Comments;
using TienDaoAPI.Enums;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly TienDaoDbContext _dbContext;
        public CommentService(IMapper mapper, TienDaoDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateCommentAsync(CreateCommentDTO dto)
        {
            try
            {
                var comment = _mapper.Map<Comment>(dto);
                _dbContext.Comments.Add(comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Comment fail : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            try
            {
                var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
                if (comment is null)
                {
                    return false;
                }
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Comment fail : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateCommentAsync(Comment comment, UpdateCommentDTO dto)
        {
            try
            {
                comment.Content = dto.Content;
                comment.UpdateAt = DateTime.UtcNow;

                _dbContext.Comments.Update(comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return false;
            }
        }

        public async Task<Comment?> GetCommentbyIdAsync(int id)
        {
            return await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>?> GetAllCommentsByBookIdAsync(int bookId, CommentFilter filter)
        {
            var sortExpression = ExpressionProvider<Comment>.GetSortExpression(filter.SortBy);
            var comments = _dbContext.Comments
                .Include(c => c.CommentLikes)
                .Include(c => c.CommentReplies).ThenInclude(cr => cr.User)
                .Include(c => c.CommentReplies).ThenInclude(cr => cr.CommentLikes)
                .Include(c => c.Chapter)
                .Include(c => c.User)
                .Where(c => c.BookId == bookId && c.CommentParentId == null);

            comments = filter.SortBy != null && filter.SortBy.StartsWith("-")
            ? comments.OrderByDescending(sortExpression)
            : comments.OrderBy(sortExpression);

            return await comments.ToListAsync();
        }

        public async Task<bool> ReplyComment(Comment comment, CreateReplyCommentDTO dto)
        {
            try
            {
                var replyComment = _mapper.Map<Comment>(dto);
                replyComment.BookId = comment.BookId;
                replyComment.ChapterId = comment.ChapterId;
                _dbContext.Comments.Add(replyComment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create reply comment fail : " + ex.Message);
                return false;
            }

        }

        public async Task<ReactionEnum> LikeOrUnlikeComment(int commentId, int userId)
        {
            try
            {
                var commentLike = await _dbContext.CommentLikes.FirstOrDefaultAsync(x => x.OwnerId == userId && x.CommentId == commentId);
                if (commentLike is null)
                {
                    var like = new CommentLike()
                    {
                        CommentId = commentId,
                        OwnerId = userId
                    };

                    _dbContext.CommentLikes.Add(like);
                    await _dbContext.SaveChangesAsync();
                    return ReactionEnum.Like;
                }
                else
                {
                    _dbContext.CommentLikes.Remove(commentLike);
                    await _dbContext.SaveChangesAsync();
                    return ReactionEnum.UnLike;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Like comment fail : " + ex.Message);
                return ReactionEnum.Fail;
            }
        }
    }
}
