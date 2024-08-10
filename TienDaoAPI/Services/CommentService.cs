using AutoMapper;
using System.Transactions;
using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly IChapterRepository _chapterRepository;
        public CommentService(
            ICommentRepository commentRepository,
            IBookRepository bookRepository,
            IChapterRepository chapterRepository,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
            _bookRepository = bookRepository;
            _chapterRepository = chapterRepository;
        }
        public async Task<Comment?> CreateCommentAsync(CreateCommentDTO dto)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var book = await  _bookRepository.GetByIdAsync(dto.BookId);
                    if(book == null)
                    {
                        Console.WriteLine("Truyện không tồn tại.");
                        return null;
                    }
                    var chapter = await _chapterRepository.GetByIdAsync(dto.ChapterId);
                    if(chapter == null)
                    {
                        Console.WriteLine("Chapter không tồn tại.");
                        return null;
                    }
                    //Mapping CommentDTO with comment
                    var comment = _mapper.Map<Comment>(dto);
                    var newComment = await _commentRepository.CreateAsync(comment);
                    if (newComment != null)
                    {
                        scope.Complete();
                    }
                    return newComment;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Comment fail : " + ex.Message);
                return null;
            }
        }
        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment != null)
            {
                await _commentRepository.DeleteAsync(comment);
                return true;
            }
            return false;
        }

        public async Task<Comment?> UpdateCommentAsync(Comment comment, UpdateCommentDTO dto)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    comment.Content = dto.Content;
                    comment.UpdateAt = DateTime.UtcNow;

                    var updateComment = await _commentRepository.UpdateAsync(comment);
                    if (updateComment != null)
                    {
                        scope.Complete();
                    }
                    return updateComment;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return null;

            }
        }
        public async Task<Comment?> GetCommentbyIdAsync(int id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Comment?>> GetAllCommentAsync(CommentFilter filter)
        {
            try
            {
                var sortExpression = filter.SortBy == null ? null : ExpressionProvider<Comment>.GetSortExpression(filter.SortBy);
                if (sortExpression == null)
                {
                    return null;
                }

                var comments = await _commentRepository.FilterAsync(null, null, sortExpression);

                return comments;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Server Error : " + ex.Message);
                return null;
            }
        }
        public async Task<Comment?> ReplyComment(CreateReplyCommentDTO dto)
        {
            var comment = await _commentRepository.GetByIdAsync(dto.CommentParentId);
            if(comment != null)
            {
                try
                {

                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        //Mapping reply comment with comment
                        var replyComment = _mapper.Map<Comment>(dto);
                        replyComment.ChapterNumber = comment.ChapterNumber;
                        replyComment.BookId = comment.BookId;
                        var createReplyComment = await _commentRepository.CreateAsync(replyComment);
                        if (createReplyComment != null)
                        {
                            scope.Complete();
                        }
                        return createReplyComment;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Create reply comment fail : " + ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<ReactionEnum?> UserLikeComment(int commentId, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if(comment != null)
            {
                var userReact = comment.UserLike.Exists(userid => userid == userId);
                if (userReact == true)
                {
                    comment.UserLike.Remove(userId);
                    await _commentRepository.UpdateAsync(comment);
                    return ReactionEnum.UnLike;
                }
                else
                {
                    comment.UserLike.Add(userId);
                    await _commentRepository.UpdateAsync(comment);
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
