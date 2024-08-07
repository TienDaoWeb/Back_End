using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IReviewService
    {
        public Task<Review?> CreateReviewAsync(CreateReviewDTO dto);
        public Task<bool> DeleteReviewAsync(Review review);
        public Task<Review?> UpdateReviewAsync(Review review, UpdateReviewDTO dto);
        public Task<ReactionEnum> UserLikeComment(Review review, int userId);
        public Task<IEnumerable<Review>> GetAllReviewAsync(ReviewFilter filter);
        public Task<Review?> GetReviewAsync(int id);
    }
}