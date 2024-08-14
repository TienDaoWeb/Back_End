using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
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

        public async Task<IEnumerable<Comment>?> GetAllCommentAsync(CommentFilter filter)
        {
            //var sortExpression = filter.SortBy == null ? null : ExpressionProvider<Comment>.GetSortExpression(filter.SortBy);

            return await _dbContext.Comments.ToListAsync();
        }

        public async Task<bool> ReplyComment(CreateReplyCommentDTO dto)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == dto.CommentParentId);
            if (comment != null)
            {
                try
                {
                    var replyComment = _mapper.Map<Comment>(dto);
                    replyComment.ChapterNumber = comment.ChapterNumber;
                    replyComment.BookId = comment.BookId;
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
            else
            {
                return false;
            }
        }

        public async Task<ReactionEnum?> UserLikeComment(int commentId, int userId)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment is not null)
            {
                var userReact = comment.UserLike.Exists(userid => userid == userId);
                if (userReact == true)
                {
                    comment.UserLike.Remove(userId);
                    _dbContext.Comments.Update(comment);
                    await _dbContext.SaveChangesAsync();
                    return ReactionEnum.UnLike;
                }
                else
                {
                    comment.UserLike.Add(userId);
                    _dbContext.Comments.Update(comment);
                    await _dbContext.SaveChangesAsync();
                    return ReactionEnum.Like;
                }
            }
            else
            {
                return ReactionEnum.Fail;
            }
        }
    }
}
