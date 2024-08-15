using TienDaoAPI.DTOs.Reviews;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services.IServices
{
    public interface IReviewService
    {
        public Task<bool> CreateReviewAsync(CreateReviewDTO dto);
        public Task<bool> DeleteReviewAsync(Review review);
        public Task<bool> UpdateReviewAsync(Review review, UpdateReviewDTO dto);
        public Task<ReactionEnum> LikeOrUnlikeReview(int reviewId, int userId);
        public Task<IEnumerable<Review>?> GetAllReviewsByBookIdAsync(int bookId, ReviewFilter filter);
        public Task<Review?> GetReviewByIdAsync(int id);
    }
}