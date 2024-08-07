using AutoMapper;
using System.Transactions;
using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public ReviewService (
            IReviewRepository reviewRepository,
            IMapper mapper
        )
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        public async Task<Review?> CreateReviewAsync(CreateReviewDTO dto)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var createReview = _mapper.Map<Review>(dto);
                    createReview.CreatedAt = DateTime.UtcNow;
                    createReview.UpdatedAt = DateTime.UtcNow;

                    var review = await _reviewRepository.CreateAsync(createReview);
                    if (review != null)
                    {
                        scope.Complete();
                    }
                    return review;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return null;
            }
        }
        public async Task<bool> DeleteReviewAsync(Review review)
        {
            try
            {
                await _reviewRepository.DeleteAsync(review);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return false;
            }
        }
        public async Task<Review?> GetReviewAsync(int id)
        {
            return await _reviewRepository.GetByIdAsync(id);

        }
        public async Task<Review?> UpdateReviewAsync(Review review , UpdateReviewDTO dto)
        {
            try
            {
                using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    _mapper.Map(dto, review);
                    review.UpdatedAt = DateTime.UtcNow;

                    var result = await _reviewRepository.UpdateAsync(review);

                    if(result != null)
                    {
                        scope.Complete();
                    }
                    return result;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return null;
            }

        }
        public async Task<ReactionEnum> UserLikeComment(Review review, int userId)
        {
            try
            {
                if (review != null)
                {
                    var userReact = review.UsersReaction.Exists(userid => userid == userId);
                    if (userReact == true)
                    {
                        review.UsersReaction.Remove(userId);
                        await _reviewRepository.UpdateAsync(review);
                        return ReactionEnum.UnLike;
                    }
                    else
                    {
                        review.UsersReaction.Add(userId);
                        await _reviewRepository.UpdateAsync(review);
                        return ReactionEnum.Like;
                    }
                }
                else
                {
                    return ReactionEnum.Fail;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return ReactionEnum.Fail;
            }
        }
        public async Task<IEnumerable<Review>> GetAllReviewAsync(ReviewFilter filter)
        {
            try
            {
                var sortExpression = filter.SortBy == null ? null : ExpressionProvider<Review>.GetSortExpression(filter.SortBy);
                if (sortExpression == null)
                {
                    return Enumerable.Empty<Review>();
                }

                var reviews = await _reviewRepository.FilterAsync(null, null, sortExpression);

                return reviews;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server Error : " + ex.Message);
                return Enumerable.Empty<Review>();
            }
        }
    }
}
