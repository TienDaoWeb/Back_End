using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs.Reviews;
using TienDaoAPI.Enums;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly TienDaoDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReviewService(IMapper mapper, TienDaoDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateReviewAsync(CreateReviewDTO dto)
        {
            try
            {
                var review = _mapper.Map<Review>(dto);
                _dbContext.Reviews.Add(review);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteReviewAsync(Review review)
        {
            try
            {
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return false;
            }
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> UpdateReviewAsync(Review review, UpdateReviewDTO dto)
        {
            try
            {
                _mapper.Map(dto, review);
                review.UpdatedAt = DateTime.UtcNow;

                _dbContext.Reviews.Update(review);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
                return false;
            }

        }

        public async Task<ReactionEnum> LikeOrUnlikeReview(int reviewId, int userId)
        {
            try
            {
                var reviewLike = await _dbContext.ReviewLikes.FirstOrDefaultAsync(x => x.OwnerId == userId && x.ReviewId == reviewId);
                if (reviewLike is null)
                {
                    var like = new ReviewLike()
                    {
                        ReviewId = reviewId,
                        OwnerId = userId
                    };

                    _dbContext.ReviewLikes.Add(like);
                    await _dbContext.SaveChangesAsync();
                    return ReactionEnum.Like;
                }
                else
                {
                    _dbContext.ReviewLikes.Remove(reviewLike);
                    await _dbContext.SaveChangesAsync();
                    return ReactionEnum.UnLike;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ReactionEnum.Fail;
            }
        }

        public async Task<IEnumerable<Review>?> GetAllReviewsByBookIdAsync(int bookId, ReviewFilter filter)
        {
            var sortExpression = ExpressionProvider<Review>.GetSortExpression(filter.SortBy);
            var reviews = _dbContext.Reviews
                .Include(c => c.ReviewLikes)
                .Include(c => c.Book)
                .Include(c => c.User)
                .Where(c => c.BookId == bookId);

            reviews = filter.SortBy != null && filter.SortBy.StartsWith("-")
            ? reviews.OrderByDescending(sortExpression)
            : reviews.OrderBy(sortExpression);

            return await reviews.ToListAsync();
        }
    }
}
